using Monocle;

namespace Gamespace.Data;

public static class Fonts {
    public static PixelFont NezFont;

    public static void Load() {
        NezFont = new PixelFont("NezDefault");
        NezFont.AddFontSize("Fonts/NezDefault.fnt");
    }
}
