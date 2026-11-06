using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public class Parallax : Backdrop {
    private readonly MTexture texture;
    public bool LoopX;

    public Parallax(string path, Vector2 position = default, Vector2 justify = default, Vector2 scroll = default, bool loopX = true) {
        Scroll = scroll;
        texture = GFX.Game["background/" + path];
        LoopX = loopX;
        Position = position - new Vector2(justify.X * texture.Width, justify.Y * texture.Height);
    }

    public override void Render(Camera camera) {
        Vector2 scroll = -camera.Position * Scroll;

        if (LoopX) {
            scroll.X = snapParallel(scroll.X, Engine.Width);

            for (; scroll.X < Engine.Width; scroll.X += texture.Width) {
                texture.Draw(Position + scroll);
            }
        } else {
            texture.Draw(Position + scroll);
        }
    }

    private static float snapParallel(float value, float windowEnd) {
        float multiple = (int) Math.Ceiling(value / windowEnd);
        return value - multiple * windowEnd;
    }
}
