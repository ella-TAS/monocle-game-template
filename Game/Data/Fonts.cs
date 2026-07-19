using Monocle;

namespace Gamespace.Data;

public static class Fonts {
    public static PixelFont NezFont;

    public static void Load() {
        NezFont = new PixelFont("Minimal");
        NezFont.AddFontSize("Fonts/minimal.fnt");
    }
}
