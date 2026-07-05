using Gamespace.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace.Scenes;

public class GameScene : Scene {
    public Camera Camera => Renderer.Camera;
    private EverythingRenderer Renderer;

    public Vector2 MousePosition => Camera.ScreenToCamera(MInput.Mouse.Position);

    public override void Begin() {
        Renderer = new EverythingRenderer() {
            SamplerState = SamplerState.PointClamp,
        };
        Add(Renderer);

        Add(new Player());
    }

    public override void Render() {
        Engine.Instance.GraphicsDevice.Clear(Color.Transparent);

        base.Render();
    }
}
