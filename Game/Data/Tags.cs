using Monocle;

namespace Gamespace;

public static class Tags {
    public static BitTag PauseUpdate;
    public static BitTag HUD;

    public static void Init() {
        PauseUpdate = new BitTag("PauseUpdate");
        HUD = new BitTag("HUD");
    }
}
