using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Gamespace;

public class GameScene : Scene {
    public override void Begin() {
        Add(new TagExcludeRenderer(Tags.HUD) {
            SamplerState = SamplerState.PointClamp,
        });
        Add(new ParallaxRenderer());
        Add(new SingleTagRenderer(Tags.HUD));

        Add(new Player());
    }

    public override void Update() {
        base.Update();

        if (Paused) {
            this[Tags.PauseUpdate].ForEach(e => e.Update());
        }

#if DEBUG
        // return to menu
        if (MInput.Keyboard.Pressed(Keys.Escape)) {
            Engine.Scene = new MenuScene();
        }
#endif
    }
}
