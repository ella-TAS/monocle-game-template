using Microsoft.Xna.Framework;

namespace Gamespace.Util;

public static class Vector2Ext {
    public static Vector2 Approach(this Vector2 val, Vector2 target, float maxMove) {
        return Calc.Approach(val, target, maxMove);
    }

    public static float Distance(this Vector2 val, Vector2 other) {
        return Vector2.Distance(val, other);
    }
}
