using Gamespace.Util;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace Gamespace.Entities;

public class Player : Entity {
    public Player() {
        Texture2D moonTex = Core.Content.LoadTexture("Graphics/heart");
        AddComponent(new SpriteRenderer(moonTex));
    }

    public override void Update() {
        base.Update();

        Position = Position.Approach(Input.MousePosition, 1f);
    }
}
