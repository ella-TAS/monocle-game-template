using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Gamespace;

public class FontTesterScene : Scene {
    public override void Begin() {
        Add(new EverythingRenderer() {
            SamplerState = SamplerState.PointClamp
        });

        Add(new FontTester());
    }

    public override void Update() {
        base.Update();
#if DEBUG
        // return to menu
        if (MInput.Keyboard.Pressed(Keys.Escape)) {
            Engine.Scene = new MenuScene();
        }
#endif
    }
}
