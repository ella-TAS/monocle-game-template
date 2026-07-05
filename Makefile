fnalibs_source=https://nightly.link/FNA-XNA/fnalibs-dailies/workflows/ci/main
fnalibs_wasm_source=https://github.com/r58Playz/FNA-WASM-Build/releases/latest/download
fxc_source=https://tail.snipundercover.ovh/public/fxc-purrtable

release_linux=Game/bin/Release/net10.0/linux-x64/publish
release_win=Game/bin/Release/net10.0/win-x64/publish
release_wasm=Game/bin/Release/net10.0/publish

FX := $(wildcard Game/Content/Effects/*.fx) $(wildcard Game/Content/Effects/Nez/*.fx)
FXB := $(FX:.fx=.fxb)

watch: artifacts
	LD_LIBRARY_PATH="$(PWD)/fnalibs/lib64/" dotnet watch --project Game/Game.csproj

run: artifacts
	LD_LIBRARY_PATH="$(PWD)/fnalibs/lib64/" dotnet run --project Game/Game.csproj

build: artifacts
	dotnet build Game/Game.csproj

wasm: artifacts wasm-build serve

wasm-build:
	# try to revert patch in case the build was cancelled
	make -i unpatch
	make patch
	dotnet publish Game/Game.Wasm.csproj -c Release
	make unpatch

	# fix mono init with -sWASMFS enabled
	sed -i 's/FS_createPath("\/","usr\/share",!0,!0)/FS_createPath("\/usr","share",!0,!0)/' $(release_wasm)/wwwroot/_framework/dotnet.runtime.*.js
	# automatically force transfer of canvas matching selector `.canvas` (class canvas) to deputy thread (c# managed main thread)
	sed -i 's/var offscreenCanvases={};/var offscreenCanvases={};if(globalThis.window\&\&!window.TRANSFERRED_CANVAS){transferredCanvasNames=[".canvas"];window.TRANSFERRED_CANVAS=true;}/' $(release_wasm)/wwwroot/_framework/dotnet.native.*.js

	# copy Content manually because the build process removes XMLs
	mv $(release_wasm)/Content $(release_wasm)/wwwroot/
	cp -r Game/Content/* $(release_wasm)/wwwroot/Content/
	rm -r $(release_wasm)/wwwroot/Content/Graphics
	find $(release_wasm)/wwwroot/Content/ -name "*.fx" -exec rm {} +
	find $(release_wasm)/wwwroot/Content/ -name ".git*" -exec rm {} +

publish-wasm: prepare-publish wasm-build
	cp -r licenses $(release_wasm)/wwwroot/
	cd $(release_wasm)/wwwroot; zip -r -9 Gamespace_web.zip *
	mv $(release_wasm)/wwwroot/*.zip ./

publish-linux: prepare-publish
	dotnet publish Game/Game.csproj -c Release -r linux-x64 --self-contained true
	chmod +x $(release_linux)/Gamespace

	cp fnalibs/lib64/* $(release_linux)/
	cp -r licenses $(release_linux)/
	cd $(release_linux); zip -r -9 Gamespace_linux.zip *
	mv $(release_linux)/*.zip ./

publish-win: prepare-publish
	dotnet publish Game/Game.csproj -c Release -r win-x64 --self-contained true

	cp fnalibs/x64/* $(release_win)/
	cp -r licenses $(release_win)/
	cd $(release_win); zip -r -9 Gamespace_windows.zip *
	mv $(release_win)/*.zip ./

publish-all: publish-linux publish-win publish-wasm

prepare-publish: qa artifacts git-reset clean

artifacts: $(FXB) crunch
	# All artifacts up-to-date

Game/Content/Effects/%.fxb: Game/Content/Effects/%.fx
	wine util/fxc/fxc.exe /T fx_2_0 $< /Fo $@

Game/Content/Effects/Nez/%.fxb: Game/Content/Effects/Nez/%.fx
	wine util/fxc/fxc.exe /T fx_2_0 $< /Fo $@

crunch:
	util/crunch Game/Content/Atlases/ Game/Content/Graphics/ -d

clean:
	rm -f *.zip
	rm -f Game/Content/Atlases/.hash
	rm -rf $(release_linux)
	rm -rf $(release_win)
	rm -rf $(release_wasm)
	find . -name "obj" -type d -exec rm -rf {} +
	find . -name "bin" -type d -exec rm -rf {} +
	# make sure Wasm patches are not applied
	make -i unpatch

patch:
	git apply --directory=FNA util/wasm/FNA.patch
	git apply --directory=monocle-engine util/wasm/Monocle.patch
	# Patches applied

unpatch:
	git apply --directory=FNA --reverse util/wasm/FNA.patch
	git apply --directory=monocle-engine --reverse util/wasm/Monocle.patch
	# Patches reverted

serve:
	python3 util/wasm/serve.py $(release_wasm)/wwwroot

qa:
	@failed=0; \
	for f in $$(find Game/ -not -path "Game/obj/*" -name '*.cs'); do \
		python3 util/quality_assurance.py "$$f" || failed=1; \
	done; \
	exit $$failed
	#############
	# QA PASSED #
	#############

setup:
	# check that basic tools exist
	wget -V
	zip -v
	unzip -v
	python3 -V
	git -v

	# initialize submodules
	make git-reset

	# install required tools
	sudo apt install -y dotnet-sdk-10.0
	sudo apt install -y dotnet-runtime-10.0
	sudo apt install -y wine
	dotnet workload install wasm-tools
	dotnet workload restore --project Game/Game.csproj
	# dotnet tool install -g dotnet-mgcb

	# download required files
	make get-libs
	make get-fxc

	# for hot reloading
	sudo sysctl fs.inotify.max_user_instances=1024

	# build all artifacts once
	make artifacts

	####################
	# SETUP SUCCESSFUL #
	####################

git-reset:
	git submodule update --init --recursive

git-pull: git-reset
	git submodule update --remote

get-libs:
	mkdir -p fnalibs
	rm -rf fnalibs/*
	make get-libs-main
	make get-libs-wasm
	# make get-libs-apple
	# FNAlibs download successful

get-libs-main:
	wget $(fnalibs_source)/fnalibs.zip -O fnalibs/fnalibs.zip
	unzip fnalibs/fnalibs.zip -d fnalibs/
	rm fnalibs/fnalibs.zip fnalibs/README.txt
	rm -r fnalibs/D3D12

get-libs-apple:
	wget $(fnalibs)/fnalibs_apple.zip -O fnalibs/fnalibs_apple.zip
	unzip fnalibs/fnalibs_apple.zip -d fnalibs/
	rm fnalibs/fnalibs_apple.zip fnalibs/README.txt

get-libs-wasm:
	mkdir fnalibs/wasm
	wget $(fnalibs_wasm_source)/FAudio.a -O fnalibs/wasm/FAudio.a
	wget $(fnalibs_wasm_source)/FNA3D.a -O fnalibs/wasm/FNA3D.a
	wget $(fnalibs_wasm_source)/libmojoshader.a -O fnalibs/wasm/libmojoshader.a
	wget $(fnalibs_wasm_source)/SDL3.a -O fnalibs/wasm/SDL3.a

get-fxc:
	mkdir -p util/fxc
	wget $(fxc_source)/D3DCompiler_43.dll -O util/fxc/D3DCompiler_43.dll
	wget $(fxc_source)/fxc.exe -O util/fxc/fxc.exe

remove-wasm:
	rm -f license/FNA-web-template.txt
	rm -f Game/Game.Wasm.csproj
	rm -rf fnalibs/wasm
	rm -rf util/wasm
	rm -rf Game/wwwroot
	echo "namespace Gamespace;\n\npublic static class Program {\n    public static void Main() {\n        using var game = new Game();\n        game.RunWithLogging(callExitOnCrash: false);\n    }\n}" > Game/Program.cs
	#
	# The following sections of the Makefile can be removed
	#
	grep "wasm" Makefile
