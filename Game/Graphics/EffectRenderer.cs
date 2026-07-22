using Gamespace.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Gamespace.Graphics;

public class DitherRenderer : Renderer {
    public Effect Effect;
    public Camera Camera;
    private readonly RenderBuffer gameBuffer;
    private readonly RenderBuffer screenBuffer;

    public DitherRenderer() {
        Effect = Effects.ForcePalette;
        Camera = new Camera();
        gameBuffer = new RenderBuffer(Engine.Width, Engine.Height);
        screenBuffer = new RenderBuffer(Engine.Width, Engine.Height);
    }

    public override void Render(Scene scene) {
        Engine.Graphics.GraphicsDevice.SetRenderTarget(gameBuffer);
        Engine.Graphics.GraphicsDevice.Clear(Engine.ClearColor);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Camera.Matrix);

        scene.Entities.Render();

        Draw.SpriteBatch.End();


        Engine.Graphics.GraphicsDevice.SetRenderTarget(screenBuffer);
        Engine.Graphics.GraphicsDevice.Clear(Engine.ClearColor);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, Effect);

        Draw.SpriteBatch.Draw(gameBuffer, Vector2.Zero, Color.White);

        Draw.SpriteBatch.End();


        Engine.Graphics.GraphicsDevice.SetRenderTarget(null);
        Engine.Graphics.GraphicsDevice.Clear(Engine.ClearColor);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);

        float scale = Engine.ViewWidth / (float) Engine.Width;
        Draw.SpriteBatch.Draw(screenBuffer, Engine.ViewportPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        if (Engine.Commands.Open) {
            scene.Entities.DebugRender(Camera);
        }

        Draw.SpriteBatch.End();
    }
}
