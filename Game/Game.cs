using Gamespace.Scenes;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Gamespace;

class Game : Core {
    public Game() : base(contentDirectory: AppContext.BaseDirectory + "/Content") {
        // pixel art upscaling
        Environment.SetEnvironmentVariable("FNA_OPENGL_BACKBUFFER_SCALE_NEAREST", "1");
        DefaultSamplerState = SamplerState.PointClamp;
    }

    override protected void Initialize() {
        base.Initialize();

        Scene = new GameScene();
    }
}
