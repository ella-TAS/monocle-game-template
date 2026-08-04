using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace;

public class PixelRenderer : Renderer {
    private readonly RenderBuffer gameBuffer;
    public Effect Effect;

    public PixelRenderer() {
        gameBuffer = new RenderBuffer(Engine.Width, Engine.Height);
    }

    public override void Render(Scene scene) {
        RenderTargetBinding[] mainTarget = Engine.Graphics.GraphicsDevice.GetRenderTargets();

        Engine.Graphics.GraphicsDevice.SetRenderTarget(gameBuffer);
        Engine.Graphics.GraphicsDevice.Clear(Engine.ClearColor);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, scene.Camera.Matrix);

        scene.Entities.Render();

        Draw.SpriteBatch.End();


        Engine.Graphics.GraphicsDevice.SetRenderTargets(mainTarget);
        Engine.Graphics.GraphicsDevice.Clear(Engine.ClearColor);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);

        float scale = Engine.ViewWidth / (float) Engine.Width;
        Draw.SpriteBatch.Draw(gameBuffer, Engine.ViewportPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        if (Engine.Commands.Open) {
            scene.Entities.DebugRender(scene.Camera);
        }

        Draw.SpriteBatch.End();
    }
}
