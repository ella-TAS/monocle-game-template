using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public class ButtonComponent : Component {
    public Action HoverAction;
    public Action HoverEndAction;
    public Action ClickAction;
    public Action ReleaseAction;

    public bool Hovering;
    public bool Clicked;
    protected Vector2 Offset;
    protected readonly Hitbox Collider;

    public ButtonComponent(float width, float height, Action releaseAction = null, Vector2 offset = default) : base(true, false) {
        Collider = new Hitbox(width, height);
        Collider.CenterOrigin();
        ReleaseAction = releaseAction;
        Offset = offset;
    }

    public override void Update() {
        base.Update();

        Collider.Center = Entity.Position + Offset;

        if (Collider.Collide(Scene.MousePosition)) {
            // hover start
            if (!Hovering) {
                Hovering = true;
                HoverAction?.Invoke();
            }

            // clicked on button
            if (MInput.Mouse.PressedLeftButton) {
                Clicked = true;
                ClickAction?.Invoke();
            }

            // released after clicking button
            if (MInput.Mouse.ReleasedLeftButton && Clicked) {
                Clicked = false;
                ReleaseAction?.Invoke();
            }
        } else {
            // reset clicking progress
            Clicked = false;

            // hover end
            if (Hovering) {
                Hovering = false;
                HoverEndAction?.Invoke();
            }
        }
    }
}
