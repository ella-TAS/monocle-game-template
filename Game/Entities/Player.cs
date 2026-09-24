using Monocle;

namespace Gamespace;

public class Player : SceneEntity<GameScene> {
    private readonly Sprite sprite;

    public Player() {
        sprite = GFX.Sprite("player");
        sprite.CenterOrigin();
        Add(sprite);
    }

    public override void Update() {
        base.Update();

        Position = Position.Approach(Scene.MousePosition, 1f);
    }
}
