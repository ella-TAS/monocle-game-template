using Gamespace.Entities;
using Nez;

namespace Gamespace.Scenes;

public class GameScene : Scene {
    public override void Initialize() {
        SetDesignResolution(Screen.Width, Screen.Height, SceneResolutionPolicy.None);

        AddEntity(new Player());
    }
}
