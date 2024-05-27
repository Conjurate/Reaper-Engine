using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reaper.Unused;

/// <summary>
/// An immutable 2D vector.
/// </summary>
public struct Vector2(float x, float y)
{
    public static Vector2 Zero => new(0, 0);
    public static Vector2 One => new(1, 1);

    public float X { get; private set; } = x;
    public float Y { get; private set; } = y;
    public float Magnitude => MathF.Sqrt((X * X) + (Y * Y));
    public System.Numerics.Vector2 ToSystem => new System.Numerics.Vector2(X, Y);

    /// <summary>
    /// Scale to unit length.
    /// </summary>
    /// <returns>Normalized vector</returns>
    public Vector2 Normalize()
    {
        float m = Magnitude;
        return m > 0 ? new(X / m, Y / m) : Zero;
    }

    /// <summary>
    /// Squared Euclidean distance between this vector and another.
    /// </summary>
    /// <returns>Squared Euclidean distance</returns>
    public float DistanceSqr(Vector2 a)
    {
        float xDiff = a.X - X;
        float yDiff = a.Y - Y;
        return (xDiff * xDiff) + (yDiff * yDiff);
    }

    /// <summary>
    /// Euclidean distance between this vector and another.
    /// </summary>
    /// <returns>Euclidean distance</returns>
    public float Distance(Vector2 a) => MathF.Sqrt(DistanceSqr(a));

    /// <summary>
    /// Dot product of this vector and another.
    /// </summary>
    /// <returns>Dot product</returns>
    public float Dot(Vector2 a) => (X * a.X) + (Y * a.Y);

    public static Vector2 operator +(Vector2 v1, Vector2 v2) => new(v1.X + v2.X, v1.Y + v2.Y);

    public static Vector2 operator -(Vector2 v1, Vector2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);

    public static Vector2 operator *(Vector2 v1, Vector2 v2) => new(v1.X * v2.X, v1.Y * v2.Y);

    public static Vector2 operator *(Vector2 v1, float scalar) => new(v1.X * scalar, v1.Y * scalar);

    public static Vector2 operator /(Vector2 v1, Vector2 v2) => new(v1.X / v2.X, v1.Y / v2.Y);

    public static Vector2 operator /(Vector2 v1, float scalar) => new(v1.X / scalar, v1.Y / scalar);
}
