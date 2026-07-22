using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace.Data;

public static class Effects {
    public static Color[] Palette;
    public static Effect ForcePalette;

    public static void Load() {
        Texture2D palette = Engine.Instance.Content.Load<Texture2D>("Effects/palette");
        Palette = new Color[palette.Width * palette.Height];
        palette.GetData(Palette);
        Vector4[] colorData = new Vector4[Palette.Length];
        for (int i = 0; i < Palette.Length; i++) {
            colorData[i] = Palette[i].ToVector4();
        }

        ForcePalette = Engine.Instance.Content.Load<Effect>("Effects/color_palette");
        ForcePalette.Parameters["Palette"].SetValue(colorData);
    }
}
