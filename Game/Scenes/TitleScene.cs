using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace;

public class TitleScene : Scene {
    private float Timer = 1f;

    public override void Begin() {
        Add(new EverythingRenderer() {
            SamplerState = SamplerState.PointClamp,
        });
        Camera.CenterOrigin();

        Entity title = new Entity();
        title.Add(new Image(GFX.Game["title"]).CenterOrigin());
        Add(title);
    }

    public override void Update() {
        base.Update();

        Timer -= Engine.DeltaTime;

        if (Timer <= 0f) {
            FadeTransition.Transition(this, new MenuScene());
        }
    }
}
