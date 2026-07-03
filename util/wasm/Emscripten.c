#include <emscripten/em_asm.h>
#include <emscripten/wasmfs.h>
#include <emscripten/html5.h>
#include <emscripten/threading.h>
#include <pthread.h>
#include <stdbool.h>
#include <assert.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <sys/stat.h>

int mount_opfs()
{
	emscripten_console_log("mount_opfs: starting");
	backend_t opfs = wasmfs_create_opfs_backend();
	emscripten_console_log("mount_opfs: created opfs backend");
	int ret = wasmfs_create_directory("/libsdl", 0777, opfs);
	emscripten_console_log("mount_opfs: mounted opfs");
	return ret;
}

backend_t fetch_backend[8] = {NULL};

int mount_fetch(int id, char *srcdir, char *dstdir)
{
	fetch_backend[id] = wasmfs_create_fetch_backend(srcdir);
	return wasmfs_create_directory(dstdir, 0777, fetch_backend[id]);
}

int mount_fetch_dir(int id, char *path)
{
	if (!fetch_backend[id])
		return -1;

	return wasmfs_create_directory(path, 0777, fetch_backend[id]);
}

int mount_fetch_file(int id, char *path)
{
	if (!fetch_backend[id])
		return -1;

	int ret = wasmfs_create_file(path, 0777, fetch_backend[id]);
	if (ret >= 0)
		return close(ret);
	return ret;
}

// needed because of upstream mono bug: https://github.com/dotnet/runtime/issues/112262
void *SDL_CreateWindow(char *title, int w, int h, uint64_t flags);
void *SDL__CreateWindow(char *title, int w, int h, unsigned int flags)
{
	return SDL_CreateWindow(title, w, h, (uint64_t)flags);
}
uint64_t SDL_GetWindowFlags(void *window);
uint32_t SDL__GetWindowFlags(void *window)
{
	return (uint32_t)SDL_GetWindowFlags(window);
}
