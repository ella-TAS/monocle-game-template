using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public static class DevCommands {
    [Command("step", "Frame step while Paused")]
    public static void FrameStep(int count = 1) {
        if (!Engine.Scene.Paused) {
            Engine.Commands.Log("Frame step only works while paused", Color.Red);
            return;
        }

        Engine.Scene.Paused = false;
        for (int i = 0; i < count; i++) {
            Engine.Scene.BeforeUpdate();
            Engine.Scene.Update();
            Engine.Scene.AfterUpdate();
        }
        Engine.Scene.Paused = true;
    }

    public static void Register() {
        // F2: frame step when paused
        Engine.Commands.FunctionKeyActions[1] = () => FrameStep();
        // F3: toggle pause
        Engine.Commands.FunctionKeyActions[2] = () => Engine.Scene.Paused = !Engine.Scene.Paused;
        // F4: toggle fullscreen
        Engine.Commands.FunctionKeyActions[3] = Game.ToggleFullscreen;
        // F5: reload scene
        Engine.Commands.FunctionKeyActions[4] = () => Engine.Scene = new GameScene();
        // F12: clear save data
        Engine.Commands.FunctionKeyActions[11] = () => SaveData.Instance = new SaveData();
    }
}
