using Monocle;

namespace Gamespace.Entities;

public class Player : Entity {
    public Player() {

    }

    public override void Update() {
        base.Update();

        Position = Position.Approach(MInput.Mouse.Position, 1f);
    }
}
