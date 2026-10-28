using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace;

public class EffectRenderer : Renderer {
    public Effect Effect;
    private readonly RenderBuffer gameBuffer;
    private readonly RenderBuffer screenBuffer;

    public EffectRenderer() {
        Effect = null;
        gameBuffer = new RenderBuffer(Engine.Width, Engine.Height);
        screenBuffer = new RenderBuffer(Engine.Width, Engine.Height);
    }

    public override void Render(Scene scene) {
        Engine.Graphics.GraphicsDevice.SetRenderTarget(gameBuffer);
        Engine.Graphics.GraphicsDevice.Clear(Color.Transparent);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, scene.Camera.Matrix);

        scene.Entities.Render();

        Draw.SpriteBatch.End();


        Engine.Graphics.GraphicsDevice.SetRenderTarget(screenBuffer);
        Engine.Graphics.GraphicsDevice.Clear(Color.Transparent);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, Effect);

        Draw.SpriteBatch.Draw(gameBuffer, Vector2.Zero, Color.White);

        Draw.SpriteBatch.End();


        Engine.ResetRenderTarget();
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Engine.ScreenMatrix);

        Draw.SpriteBatch.Draw(screenBuffer, Vector2.Zero, Color.White);
        if (Engine.Commands.Open) {
            scene.Entities.DebugRender(scene.Camera);
        }

        Draw.SpriteBatch.End();
    }

    public override void Dispose() {
        gameBuffer.Dispose();
        screenBuffer.Dispose();
    }
}
