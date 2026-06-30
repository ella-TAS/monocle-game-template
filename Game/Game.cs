using Nez;

namespace Gamespace;

class Game1 : Core {
    public Game1() : base() {
        // scaled pixel art games
        System.Environment.SetEnvironmentVariable("FNA_OPENGL_BACKBUFFER_SCALE_NEAREST", "1");
    }

    override protected void Initialize() {
        base.Initialize();

        Scene = new DefaultScene();

#if DEBUG
        System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(System.Console.Out));
#endif
    }
}