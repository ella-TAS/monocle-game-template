using Gamespace.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace.Scenes;

public class GameScene : Scene {
    public override void Begin() {
        Add(new Player());
        Add(new EverythingRenderer());
    }

    public override void Render() {
        Engine.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

        base.Render();
    }
}
