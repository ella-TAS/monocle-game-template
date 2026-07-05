using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace.Entities.UI;

public abstract class GenericButton : Entity {
    public Action ButtonAction;
    public bool Hovering;
    public bool Clicked;

    protected static Vector2 MousePosition => MInput.Mouse.Position;

    public GenericButton(Vector2 center, float width, float height, Action ReleaseAction) : base(center) {
        Collider = new Hitbox(width, height);
        Collider.CenterOrigin();
        ButtonAction = ReleaseAction;
    }

    public override void Update() {
        base.Update();

        if (Collider.Collide(MousePosition)) {
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

    public virtual void OnHover() {

    }

    public virtual void OnHoverEnd() {

    }

    public virtual void OnClick() {

    }

    public virtual void OnRelease() {
        ButtonAction();
    }
}
