using Gamespace.Entities.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace.Scenes;

public class MenuScene : Scene {
    private EverythingRenderer Renderer;

    public override void Begin() {
        Renderer = new EverythingRenderer() {
            SamplerState = SamplerState.PointClamp,
        };
        Add(Renderer);

        Add(new MenuButton(new Vector2(50, 50), "Play",
            () => FadeTransition.Transition(this, new GameScene())
        ));
        Add(new MenuButton(new Vector2(100, 50), "Exit", Engine.Instance.Exit));
    }

}
