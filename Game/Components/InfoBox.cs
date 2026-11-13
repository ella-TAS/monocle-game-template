using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public class InfoBox : NineSliceBox {
    private const int PADDING = 4;

    private readonly PixelFont Font;
    private readonly string Text;
    private float scaleTarget, scale;

    public InfoBox(string path, string text, PixelFont font) : base(path, 1, 1) {
        Font = font;
        Text = text;
        Point size = font.Data.Measure(text).RoundToPoint();
        Width = size.X + PADDING * 2;
        Height = size.Y + PADDING * 2 - 1;
    }

    public override void Update() {
        base.Update();

        scaleTarget = Visible ? 1f : 0f;

        scale += (scaleTarget - scale) * 0.3f;
        if (Math.Abs(scaleTarget - scale) <= 0.005f) {
            scale = scaleTarget;
        }

        Scale = scale * Vector2.One;
    }

    public override void Render() {
        RenderPosition = RenderPosition.Clamp(
            Scene.Camera.TopLeft,
            Scene.Camera.BottomRight - new Vector2(Width, Height) * Scale
        );

        base.Render();
        Font.Draw(Text, RenderPosition - Origin * Scale + new Vector2(PADDING) * Scale, Vector2.Zero, Scale, Color.Black);
    }
}
