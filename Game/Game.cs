using Gamespace.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Gamespace;

class Game : Engine {
    public Game() : base(320, 180, 1280, 720, "Gamespace", false) {
        Window.AllowUserResizing = false;
        IsMouseVisible = true;
        ExitOnEscapeKeypress = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

        // pixel art upscaling
        Environment.SetEnvironmentVariable("FNA_OPENGL_BACKBUFFER_SCALE_NEAREST", "1");

#if !DEBUG
        SetFullscreen();
#endif
    }

    protected override void Initialize() {
        base.Initialize();

        Scene = new GameScene();

#if !DEBUG
        Commands.Enabled = false;
#endif
    }

    protected override void LoadContent() {
        base.LoadContent();


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

        // UserIO.Save(SaveData.Instance);
    }

    public static void ToggleFullscreen() {
        if (Graphics.IsFullScreen) {
            SetWindowed(1280, 720);
        } else {
            SetFullscreen();
        }
    }
}
