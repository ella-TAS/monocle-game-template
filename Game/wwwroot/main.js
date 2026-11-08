const wasm = await eval(`import("./_framework/dotnet.js")`);
const dotnet = wasm.dotnet;

const infoText = document.getElementById("info");
infoText.textContent = "Initializing .NET";
console.log("initializing dotnet");
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

console.log("PreInit...");
await runtime.runMain();

// find content base folder
infoText.textContent = "Loading Content";
var loc = window.location.pathname;
var dir = loc.substring(0, loc.lastIndexOf('/'));
const files = await fetch("./Content/manifest.json").then(r => r.json());
await exports.Program.PreInit(dir, files, str => infoText.textContent = str);
console.log("dotnet initialized");

infoText.textContent = "Starting Game";
console.log("Init...");
await exports.Program.Init();

console.log("MainLoop...");
const main = async () => {
	const ret = await exports.Program.MainLoop(window.innerWidth, window.innerHeight);

	if (!ret) {
		console.log("Cleanup...");
		await exports.Program.Cleanup();
		return;
	}

	requestAnimationFrame(main);
}
// repeatedly run Program main loop
infoText.remove();
requestAnimationFrame(main);
