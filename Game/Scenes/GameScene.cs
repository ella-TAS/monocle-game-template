using Gamespace.Entities;
using Monocle;

namespace Gamespace.Scenes;

public class GameScene : Scene {
    public override void Begin() {
        Add(new Player());
    }
}
