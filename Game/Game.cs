using Gamespace.Data;
using Gamespace.Save;
using Gamespace.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using System.Diagnostics;

namespace Gamespace;

class Game : Engine {
    public Game() : base(320, 180, 1280, 720, "Gamespace", false) {
        Window.AllowUserResizing = false;
        IsMouseVisible = true;
        ExitOnEscapeKeypress = true;

        // fixed framerate at 60 fps
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

#if DEBUG
        Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
#elif !WASM
        SetFullscreen();
#endif
    }

    protected override void Initialize() {
        Calc.ReleaseLog("Gamespace", "Starting Monocle Game Engine");

        base.Initialize();

        SaveData.Load();
        Scene = new GameScene();

#if !DEBUG
        Commands.Enabled = false;
#endif
    }

    protected override void LoadContent() {
        Stopwatch contentLoad = Stopwatch.StartNew();

        base.LoadContent();
        GFX.Load();

        contentLoad.Stop();
        Calc.ReleaseLog("Gamespace", $"Content loaded in {contentLoad.ElapsedMilliseconds} ms");
    }

    protected override void Update(GameTime gameTime) {
        base.Update(gameTime);

        // fullscreen toggle
        if (MInput.Keyboard.Pressed(Keys.F4)) {
            ToggleFullscreen();
        }
    }

    protected override void OnExiting(object sender, EventArgs args) {
        base.OnExiting(sender, args);

        SaveData.Save();
    }

    public static void ToggleFullscreen() {
        if (Graphics.IsFullScreen) {
            SetWindowed(1280, 720);
        } else {
            SetFullscreen();
        }
    }

    public static (int Width, int Height) GetMax169Size(int maxWidth, int maxHeight) {
        float ratio = 16f / 9f;
        if (maxWidth * 9 < maxHeight * 16) {
            return (maxWidth, (int) (maxWidth / ratio));
        }
        return ((int) (maxHeight * ratio), maxHeight);
    }

    public static void Resize(int width, int height) {
        (int w, int h) = GetMax169Size(width, height);
        SetWindowed(w, h);
    }
}
