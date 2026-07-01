fnalibs_source=https://nightly.link/FNA-XNA/fnalibs-dailies/workflows/ci/main
fnalibs_wasm_source=https://github.com/r58Playz/FNA-WASM-Build/releases/latest/download
fxc_source=https://tail.snipundercover.ovh/public/fxc-purrtable

FX := $(wildcard Game/Content/Effects/*.fx)
FXC := $(FX:.fx=.fxc)

watch: $(FXC)
	LD_LIBRARY_PATH="$(PWD)/fnalibs/lib64/" dotnet watch --project Game/Game.csproj

Game/Content/Effects/%.fxc: Game/Content/Effects/%.fx
	wine util/fxc/fxc.exe /T fx_2_0 $< /Fo $@

clean:
	find . -name obj -type d -exec rm -rf {} +
	find . -name bin -type d -exec rm -rf {} +

qa:
	# namespace matches folder structure
	# class name matches file name

setup: update-deps get-fxc
	sudo apt install -y dotnet-sdk-10.0
	sudo apt install -y doD3DCompiler_43.dlltnet-runtime-8.0
	sudo apt install -y wine
	# for hot reloading
	sudo sysctl fs.inotify.max_user_instances=1024

update-deps: get-libs git-update

git-update:
	git submodule update --init --remote
	git submodule update --init --recursive

get-libs:
	mkdir -p fnalibs
	rm -rf fnalibs/*
	make get-libs-main
	make get-libs-wasm
	# make get-libs-apple

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
