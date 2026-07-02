fnalibs_source=https://nightly.link/FNA-XNA/fnalibs-dailies/workflows/ci/main
fnalibs_wasm_source=https://github.com/r58Playz/FNA-WASM-Build/releases/latest/download
fxc_source=https://tail.snipundercover.ovh/public/fxc-purrtable

publish_linux=Game/bin/Release/net8.0/linux-x64/publish
publish_win=Game/bin/Release/net8.0/win-x64/publish

FX := $(wildcard Game/Content/Effects/*.fx)
FXC := $(FX:.fx=.fxc)

watch: artifacts
	LD_LIBRARY_PATH="$(PWD)/fnalibs/lib64/" dotnet watch --project Game/Game.csproj

publish-linux: prepare-publish
	rm -rf $(publish_linux)
	dotnet publish Game/Game.csproj -c Release -r linux-x64 --self-contained true
	cp fnalibs/lib64/* $(publish_linux)/
	chmod +x $(publish_linux)/Gamespace

publish-win: prepare-publish
	rm -rf $(publish_win)
	dotnet publish Game/Game.csproj -c Release -r win-x64 --self-contained true
	cp fnalibs/x64/* $(publish_win)/

prepare-publish: artifacts git-submodule-reset qa
artifacts: $(FXC)
	# All artifacts up-to-date

Game/Content/Effects/%.fxc: Game/Content/Effects/%.fx
	wine util/fxc/fxc.exe /T fx_2_0 $< /Fo $@

clean:
	find . -name "obj" -type d -exec rm -rf {} +
	find . -name "bin" -type d -exec rm -rf {} +

qa:
	@failed=0; \
	for f in $$(find Game/ -not -path "Game/obj/*" -name '*.cs'); do \
		python3 util/quality_assurance.py "$$f" || failed=1; \
	done; \
	exit $$failed
	#############
	# QA PASSED #
	#############

setup: git-submodule-reset
	# check that basic tools exist
	wget -V
	zip -v
	unzip -v
	git -v
	python3 -V
	# install required tools
	sudo apt install -y dotnet-sdk-10.0
	sudo apt install -y dotnet-runtime-8.0
	sudo apt install -y wine
	# download required files
	make get-libs
	make get-fxc
	# for hot reloading
	sudo sysctl fs.inotify.max_user_instances=1024
	####################
	# SETUP SUCCESSFUL #
	####################

git-submodule-reset:
	git submodule update --init --recursive

git-submodule-pull: git-submodule-reset
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
