#pragma warning disable IDE0005
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
#pragma warning restore

[assembly: System.Runtime.Versioning.SupportedOSPlatform("browser")]

namespace Gamespace;

#if !BROWSER

public static partial class Program {
    public static void Main() {
        using var game = new Game();
        game.Run();
    }
}

#else

public static partial class Program {
    private static void Main() {
        Console.WriteLine("Hi!");
    }

    static Game game;
    static FieldInfo RunApplication;
    private static int lastWidth, lastHeight;

    [JSExport]
    internal static Task PreInit(string basePath) {
        return Task.Run(() => {
            Console.WriteLine("calling mount_opfs");
            int ret = Emscripten.mount_opfs();
            Console.WriteLine($"called mount_opfs: {ret}");
            if (ret != 0) {
                throw new Exception("Failed to mount OPFS");
            }

            Environment.SetEnvironmentVariable("FNA_PLATFORM_BACKEND", "SDL3");

            // fetch all content

            string monocle = "./Content/Monocle";
            string atlas = "./Content/Atlas";
            // string effects = "./Content/Effects";
            // string audio = "./Content/Audio";

            Console.WriteLine("Content Path: " + basePath + "/Content");

            Emscripten.MountFetch(0, basePath + "/Content", "./Content");

            Emscripten.MountFetchFile(0, "./Content/Sprites.xml");

            Emscripten.MountFetchDir(0, monocle);
            Emscripten.MountFetchFile(0, monocle + "/MonocleDefault.xnb");

            Emscripten.MountFetchDir(0, atlas);
            Emscripten.MountFetchFile(0, atlas + "/0.png");
            Emscripten.MountFetchFile(0, atlas + "/.xmlx");

            // Emscripten.MountFetchDir(0, effects);
            // Emscripten.MountFetchFile(0, effects + "/menuBg.fxb");

            // Emscripten.MountFetchDir(0, audio);
            // Emscripten.MountFetchFile(0, audio + "/sound.wav");
        });
    }

    [JSExport]
    internal static Task Init() {
        // Any init for the Game - usually before game.Run() in the decompilation
        Console.WriteLine($"read test.txt from opfs: {File.ReadAllText("/libsdl/test.txt")}");
        try {
            game = new Game();
            RunApplication = game.GetType().GetField("RunApplication", BindingFlags.NonPublic | BindingFlags.Instance);
        } catch (Exception e) {
            Console.Error.WriteLine("Error while initializing!");
            Console.Error.WriteLine(e);
        }
        return Task.Delay(0);
    }

    [JSExport]
    internal static Task<bool> Cleanup() {
        // Any cleanup for the Game - usually after game.Run() in the decompilation
        return Task.FromResult(true);
    }

    [JSExport]
    internal static Task<bool> MainLoop(int width, int height) {
        try {
            if (width != lastWidth || height != lastHeight) {
                Game.Resize(width, height);
            }
            lastWidth = width;
            lastHeight = height;
            game.RunOneFrame();
        } catch (Exception e) {
            Console.Error.WriteLine("Error in MainLoop()!");
            Console.Error.WriteLine(e);
            return (Task<bool>) Task.FromException(e);
        }
        return Task.FromResult((bool) RunApplication.GetValue(game));
    }
}

public static class Emscripten {
    [DllImport("Emscripten")]
    public extern static int mount_opfs();
    [DllImport("Emscripten")]
    private extern static int mount_fetch(int id, string srcdir, string dstdir);
    [DllImport("Emscripten")]
    private extern static int mount_fetch_file(int id, string path);
    [DllImport("Emscripten")]
    private extern static int mount_fetch_dir(int id, string path);

    public static void MountFetch(int id, string src, string dst) {
        int ret = mount_fetch(id, src, dst);
        if (ret != 0) {
            throw new Exception($"Failed to mount FetchFS from {src} to {dst}: {ret}");
        }
    }

    public static void MountFetchFile(int id, string path) {
        int ret = mount_fetch_file(id, path);
        if (ret != 0) {
            throw new Exception($"Failed to mount FetchFS file at {path}: {ret}");
        }
    }

    public static void MountFetchDir(int id, string path) {
        int ret = mount_fetch_dir(id, path);
        if (ret != 0) {
            throw new Exception($"Failed to mount FetchFS directory at {path}: {ret}");
        }
    }
}

#endif
