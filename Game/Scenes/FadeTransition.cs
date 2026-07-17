using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace.Scenes;

public class FadeTransition : SceneTransition {
    public FadeTransition(Scene fromScene, Scene toScene) : base(fromScene, toScene, 2f) { }

    public static void Transition(Scene fromScene, Scene toScene) {
        new FadeTransition(fromScene, toScene).Start();
    }

    public override void Render() {
        base.Render();

        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

        Draw.SpriteBatch.Draw(FromBuffer, Engine.ViewportPosition, Color.White);
        Draw.SpriteBatch.Draw(ToBuffer, Engine.ViewportPosition, Color.White * Progress);

        Draw.SpriteBatch.End();
    }
}
