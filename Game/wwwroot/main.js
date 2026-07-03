
const wasm = await eval(`import("./_framework/dotnet.js")`);
const dotnet = wasm.dotnet;

console.debug("initializing dotnet");
const runtime = await dotnet.withConfig({}).create();

const config = runtime.getConfig();
const exports = await runtime.getAssemblyExports(config.mainAssemblyName);
const canvas = document.getElementById("canvas");
dotnet.instance.Module.canvas = canvas;

self.wasm = {
	Module: dotnet.instance.Module,
	dotnet,
	runtime,
	config,
	exports,
	canvas,
};

let opfs = await navigator.storage.getDirectory();
// writing to opfs will put it at /libsdl/... in c#
let file = await opfs.getFileHandle("test.txt", { create: true });
let writable = await file.createWritable();
await writable.write(new TextEncoder().encode("test data")); // or pipeTo a readablestream from Response.body etc
await writable.close();

console.debug("PreInit...");
await runtime.runMain();

// find content base folder
var loc = window.location.pathname;
var dir = loc.substring(0, loc.lastIndexOf('/'));
await exports.Program.PreInit(dir);
console.debug("dotnet initialized");

console.debug("Init...");
await exports.Program.Init();

console.debug("MainLoop...");
const main = async () => {
	const ret = await exports.Program.MainLoop(window.innerWidth, window.innerHeight);

	if (!ret) {
		console.debug("Cleanup...");
		await exports.Program.Cleanup();
		return;
	}

	requestAnimationFrame(main);
}
// repeatedly run Program main loop
requestAnimationFrame(main);
