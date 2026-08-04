using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace;

public class ParallaxRenderer : Renderer {
    public List<Backdrop> Backdrops = [];
    public Effect Effect;

    public ParallaxRenderer() {
        Effect = null;
    }

    public override void Update(Scene scene) {
        base.Update(scene);

        Backdrops.ForEach(b => b.Update());
    }

    public override void Render(Scene scene) {
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, Effect, Engine.ScreenMatrix);

        foreach (Backdrop backdrop in Backdrops)
            if (backdrop.Visible)
                backdrop.Render(scene.Camera);

        Draw.SpriteBatch.End();
    }
}
