using Microsoft.Xna.Framework;
using Nez;

namespace Gamespace.Util;

public static class Vector2Ext {
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

    public static Vector2 Rotate(this Vector2 val, float angleRadians) {
        return Mathf.AngleToVector(val.Angle() + angleRadians, val.Length());
    }

    public static Vector2 RotateTowards(this Vector2 val, float targetAngleRadians, float maxMoveRadians) {
        float angle = Mathf.ApproachAngleRadians(val.Angle(), targetAngleRadians, maxMoveRadians);
        return Mathf.AngleToVector(angle, val.Length());
    }

    public static float Angle(this Vector2 val) {
        return Mathf.Atan2(val.Y, val.X);
    }

    public static Vector2 Clamp(this Vector2 val, Vector2 min, Vector2 max) {
        return Vector2.Clamp(val, min, max);
    }

    public static float Distance(this Vector2 val, Vector2 other) {
        return Vector2.Distance(val, other);
    }

    public static Vector2 XComp(this Vector2 val) {
        return Vector2.UnitX * val.X;
    }

    public static Vector2 YComp(this Vector2 val) {
        return Vector2.UnitY * val.Y;
    }
}
