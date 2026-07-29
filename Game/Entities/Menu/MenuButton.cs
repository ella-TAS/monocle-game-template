using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public class MenuButton : GenericButton {
    private readonly PixelText buttonText;

    private float scaleTarget = 1f;
    private float scale = 1f;

    public MenuButton(Vector2 center, string text, Action ReleaseAction) : base(center, 32, 16, ReleaseAction) {
        buttonText = new PixelText(Fonts.Minor, Vector2.Zero, text, Color.Green);
        Add(buttonText);
    }

    public override void Update() {
        base.Update();

        scale += (scaleTarget - scale) * 0.1f;
        if (Math.Abs(scaleTarget - scale) <= 0.01f) {
            scale = scaleTarget;
        }

        buttonText.Scale = Vector2.One * scale;
    }

    public override void OnHover() {
        scaleTarget = 1.125f;
    }

    public override void OnHoverEnd() {
        scaleTarget = 1f;
    }
}
