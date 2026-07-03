using Gamespace.Data;
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

        Position = Position.Approach(MInput.Mouse.Position, 1f);
    }
}
