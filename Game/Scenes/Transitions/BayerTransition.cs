using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace.Scenes.Transitions;

public class BayerTransition : SceneTransition {
    public BayerTransition(Scene fromScene, Scene toScene) : base(fromScene, toScene, 1f) { }

    public static void Transition(Scene fromScene, Scene toScene) {
        new BayerTransition(fromScene, toScene).Start();
    }

    public override void Render() {
        base.Render();

        // Effects.BayerTransition.Parameters["Progress"].SetValue(Progress);

        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
        Draw.SpriteBatch.Draw(FromBuffer, Vector2.Zero, Color.White);
        Draw.SpriteBatch.End();

        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null); // Effects.BayerTransition);
        Draw.SpriteBatch.Draw(ToBuffer, Vector2.Zero, Color.White * Progress);
        Draw.SpriteBatch.End();
    }
}
