using Microsoft.Xna.Framework;
using Monocle;

namespace Gamespace;

public class Backdrop {
    public bool Visible;
    public Vector2 Position;
    public Vector2 Scroll;

    public virtual void Update() {

    }

    public virtual void Render(Camera camera) {

    }
}
