using Monocle;

namespace Gamespace;

public static class Fonts {
    public static PixelFont Nano;
    public static PixelFont Minor;
    public static PixelFont MinorBold;
    public static PixelFont Medion;

    public static void Load() {
        Nano = LoadFont("PxNano");
        Minor = LoadFont("PxMinor");
        MinorBold = LoadFont("PxMinorBold");
        Medion = LoadFont("PxMedion");
    }

    public static PixelFont LoadFont(string name) {
        PixelFont font = new PixelFont(name);
        font.AddFontSize($"Dialog/Fonts/{name}.fnt");
        return font;
    }
}
