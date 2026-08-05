using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace;

public class MenuScene : Scene {
    public override void Begin() {
        Add(new EverythingRenderer() {
            SamplerState = SamplerState.PointClamp,
        });

        Add(new MenuButton(new Vector2(50, 50), "Play",
            () => FadeTransition.Transition(this, new GameScene())
        ));
        Add(new MenuButton(new Vector2(100, 50), "Exit", Engine.Instance.Exit));

        Add(new InfoBox());
    }
}
