using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public class FontTester : Entity {
    private const int DIST = 20;

    public static readonly PixelFont Font = Fonts.Nano;
    private readonly List<char> Chars;
    private readonly string Feature;

    public FontTester(string feature = "") {
        Chars = Font.Data.Characters.Select(kv => (char) kv.Value.Character).ToList();
        Feature = feature;

        Vector2 pos = DIST * Vector2.One;
        foreach (char c in Chars) {
            Add(new PixelText(Font, pos, Feature + c + Feature, Color.White));

            pos.X += DIST;
            if (pos.X + DIST > Engine.Width) {
                pos.X = DIST;
                pos.Y += DIST;
            }
        }
    }

    public override void Update() {
        base.Update();

        if (Feature == "" && MInput.Mouse.PressedLeftButton) {
            string next = Components.Cast<PixelText>().MinBy(c => (c.Position - Scene.MousePosition).LengthSquared()).Text;
            Scene.Add(new FontTester(next));
            RemoveSelf();
        }
    }
}
