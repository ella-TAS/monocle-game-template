using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public class ButtonComponent : Component {
    public Action ButtonAction;
    public bool Hovering;
    public bool Clicked;
    protected Vector2 Offset;
    protected readonly Hitbox Collider;

    public ButtonComponent(float width, float height, Action releaseAction, Vector2 offset = default) : base(true, false) {
        Collider = new Hitbox(width, height);
        Collider.CenterOrigin();
        ButtonAction = releaseAction;
        Offset = offset;
    }

    public override void Update() {
        base.Update();

        Collider.Center = Entity.Position + Offset;

        if (Collider.Collide(Scene.MousePosition)) {
            // hover start
            if (!Hovering) {
                Hovering = true;
                OnHover();
            }

            // clicked on button
            if (MInput.Mouse.PressedLeftButton) {
                Clicked = true;
                OnClick();
            }

            // released after clicking button
            if (MInput.Mouse.ReleasedLeftButton && Clicked) {
                Clicked = false;
                OnRelease();
            }
        } else {
            // reset clicking progress
            Clicked = false;

            // hover end
            if (Hovering) {
                Hovering = false;
                OnHoverEnd();
            }
        }
    }

    public void OnHover() {

    }

    public void OnHoverEnd() {

    }

    public void OnClick() {

    }

    public void OnRelease() {
        ButtonAction();
    }
}
