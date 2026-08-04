using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Gamespace;

public class GameScene : Scene {
    private TagExcludeRenderer Renderer;

    public Vector2 MousePosition => Camera.ScreenToCamera(MInput.Mouse.Position);

    public override void Begin() {
        Renderer = new TagExcludeRenderer(Tags.HUD) {
            SamplerState = SamplerState.PointClamp,
        };
        Add(new ParallaxRenderer());
        Add(Renderer);
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
