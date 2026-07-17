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
        Window.AllowUserResizing = true;
        IsMouseVisible = true;
        ExitOnEscapeKeypress = false;

        // fixed framerate at 60 fps
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

        ClearColor = Color.CornflowerBlue;
    }

    protected override void Initialize() {
        base.Initialize();

        SaveData.Load();
        Scene = new MenuScene();

#if !DEBUG
        Commands.Enabled = false;
#endif
#if !DEBUG && !WASM
        SetFullscreen();
#endif
    }

    protected override void LoadContent() {
        Stopwatch contentLoad = Stopwatch.StartNew();

        base.LoadContent();
        GFX.Load();
        Fonts.Load();

        contentLoad.Stop();
        Logger.Release("Monocle", $"Content loaded in {contentLoad.ElapsedMilliseconds} ms");
    }

    protected override void Update(GameTime gameTime) {
        base.Update(gameTime);

#if DEBUG
        // fullscreen toggle
        if (MInput.Keyboard.Pressed(Keys.F4)) {
            ToggleFullscreen();
        }

        // debug reset
        if (MInput.Keyboard.Pressed(Keys.R)) {
            Scene = new MenuScene();
        }

        // debug save data reset
        if (MInput.Keyboard.Pressed(Keys.S)) {
            SaveData.Instance = new SaveData();
        }
#endif
    }

    protected override void OnExiting(object sender, EventArgs args) {
        base.OnExiting(sender, args);

        // in order to save on crash, pass callExitOnCrash: true in Program.cs
        SaveData.Save();
    }

    public static void ToggleFullscreen() {
        if (Graphics.IsFullScreen) {
            SetWindowed(1280, 720);
        } else {
            SetFullscreen();
        }
    }

    // resizing the game in the browser window
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
