using Monocle;

namespace Gamespace.Data;

public static class Fonts {
    public static PixelFont Nano;
    public static PixelFont Minor;
    public static PixelFont MinorBold;
    public static PixelFont Medion;

    public static void Load() {
        Nano = new PixelFont("PxNano");
        Nano.AddFontSize("Fonts/PxNano.fnt");
        Minor = new PixelFont("PxMinor");
        Minor.AddFontSize("Fonts/PxMinor.fnt");
        MinorBold = new PixelFont("PxMinorBold");
        MinorBold.AddFontSize("Fonts/PxMinorBold.fnt");
        Medion = new PixelFont("PxMedion");
        Medion.AddFontSize("Fonts/PxMedion.fnt");
    }
}
