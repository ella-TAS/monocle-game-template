using Monocle;

namespace Gamespace;

public static class Fonts {
    public static PixelFont Nano;
    public static PixelFont Minor;
    public static PixelFont MinorBold;
    public static PixelFont Medion;

    public static void Load() {
        Nano = new PixelFont("Dialog/Fonts/PxNano.fnt");
        Minor = new PixelFont("Dialog/Fonts/PxMinor.fnt");
        MinorBold = new PixelFont("Dialog/Fonts/PxMinorBold.fnt");
        Medion = new PixelFont("Dialog/Fonts/PxMedion.fnt");
    }
}
