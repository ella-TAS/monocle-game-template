using Gamespace.Data;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace.Scenes;

public class TitleScene : Scene {
    private EverythingRenderer Renderer;

    private float Timer = 1f;

    public override void Begin() {
        Renderer = new EverythingRenderer() {
            SamplerState = SamplerState.PointClamp,
        };
        Renderer.Camera.CenterOrigin();
        Add(Renderer);

        Entity title = new Entity();
        title.Add(new Image(GFX.Game["title"]).CenterOrigin());
        Add(title);
    }

    public override void Update() {
        base.Update();

        Timer -= Engine.DeltaTime;

        if (Timer <= 0f) {
            Engine.Scene = new MenuScene();
        }
    }
}
