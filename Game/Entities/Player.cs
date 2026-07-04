using Gamespace.Data;
using Gamespace.Scenes;
using Monocle;

namespace Gamespace.Entities;

public class Player : Entity {
    private readonly Sprite sprite;

    public Player() {
        sprite = GFX.Sprites.Create("player");
        sprite.CenterOrigin();
        Add(sprite);
    }

    public override void Update() {
        base.Update();

        Position = Position.Approach(SceneAs<GameScene>().MousePosition, 1f);
    }
}
