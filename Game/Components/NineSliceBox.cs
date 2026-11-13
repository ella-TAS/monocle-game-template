using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public class NineSliceBox : GraphicsComponent {
    public int Width;
    public int Height;
    private readonly MTexture[,] nineSlice;
    private readonly Point tileSize;

    public NineSliceBox(string path, int boxWidth, int boxHeight, Vector2 offset = default) : base(true) {
        Position = offset;
        Width = boxWidth;
        Height = boxHeight;

        nineSlice = new MTexture[3, 3];
        MTexture texture = GFX.Game[path];
        if (texture.Width % 3 != 0 || texture.Height % 3 != 0) {
            Logger.DebugCrash("NineSliceBox", $"Texture ({path}) dimensions not divisible by 3.");
        }
        tileSize = (new Vector2(texture.Width, texture.Height) / 3).RoundToPoint();
        for (int r = 0; r < 3; r++) {
            for (int c = 0; c < 3; c++) {
                nineSlice[r, c] = texture.GetSubtexture(new Rectangle(c * tileSize.X, r * tileSize.Y, tileSize.X, tileSize.Y));
            }
        }
    }

    public override void Render() {
        base.Render();

        Vector2 drawPos = RenderPosition - Origin * Scale;

        int rows = (int) MathF.Ceiling((float) Height / tileSize.Y);
        int cols = (int) MathF.Ceiling((float) Width / tileSize.X);


        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < cols; c++) {
                Vector2 offset = new Vector2(c * tileSize.X * Scale.X, r * tileSize.Y * Scale.Y);
                if (c == cols - 1 && cols > 1) offset.X = (Width - tileSize.X) * Scale.X;
                if (r == rows - 1 && rows > 1) offset.Y = (Height - tileSize.Y) * Scale.Y;
                // TODO: Rotation isn't as simple as the following, but could be done
                // selectTile(r, c, rows, cols).Draw(offset.Rotate(Rotation), (-drawPos / Scale).Rotate(-Rotation), Color, Scale, Rotation);
                selectTile(r, c, rows, cols).Draw(offset, -drawPos / Scale, Color, Scale);
            }
        }
    }

    private MTexture selectTile(int r, int c, int rows, int cols) {
        return nineSlice[selectIndex(r, rows), selectIndex(c, cols)];
    }

    private static int selectIndex(int i, int len) {
        if (i == 0) return 0;
        return i < len - 1 ? 1 : 2;
    }

    public void CenterOrigin() {
        Origin = new Vector2(Width, Height) / 2;
    }
}
