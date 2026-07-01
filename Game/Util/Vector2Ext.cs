using Microsoft.Xna.Framework;

namespace Gamespace.Util;

public static class Vector2Extension {
    private const float EPSILON = 0.00001f;

    public static Vector2 SafeNormalize(this Vector2 val, float length = 1f) {
        float size = val.Length();
        if (size <= EPSILON) {
            return Vector2.Zero;
        }

        return val / size * length;
    }

    public static Vector2 Approach(this Vector2 val, Vector2 target, float maxMove) {
        if (maxMove <= 0f || val == target) {
            return val;
        }

        Vector2 diff = target - val;
        float length = diff.Length();

        if (length < maxMove) {
            return target;
        } else {
            return val + diff.SafeNormalize() * maxMove;
        }
    }
}
