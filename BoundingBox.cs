using Raylib_cs;
using System.Drawing;
using System.Numerics;
using System.Xml.Schema;

namespace Reaper;

public readonly struct BoundingBox : IEquatable<BoundingBox>
{
    public static BoundingBox FromCenter(BoundingBox box, Vector2 pos)
    {
        Vector2 halfSize = box.Size * 0.5f;
        Vector2 min = pos - halfSize;
        Vector2 max = pos + halfSize;
        return new BoundingBox(min, max);
    }

    public Vector2 Min { get; }
    public Vector2 Max { get; }
    public float Width => Max.X - Min.X;
    public float Height => Max.Y - Min.Y;
    public Vector2 Size => Max - Min;

    public BoundingBox(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }

    public BoundingBox(float xMin, float yMin, float xMax, float yMax) : this(new Vector2(xMin, yMin), new Vector2(xMax, yMax)) { }

    public bool Contains(BoundingBox box) => Min.X <= box.Min.X && Max.X >= box.Max.X && Min.Y <= box.Min.Y && Max.Y >= box.Max.Y;

    public bool Contains(float x, float y) => Min.X <= x && Max.X >= x && Min.Y <= y && Max.Y >= y;

    public bool Contains(Vector2 pos) => Contains(pos.X, pos.Y);

    public bool Intersects(float xMin, float yMin, float xMax, float yMax) => Min.X <= xMax && Max.X >= xMin && Min.Y <= yMax && Max.Y >= yMin;

    public bool Intersects(Vector2 min, Vector2 max) => Intersects(min.X, min.Y, max.X, max.Y);

    public bool Intersects(BoundingBox box) => Intersects(box.Min, box.Max);

    public override bool Equals(object obj) => obj is BoundingBox other && Equals(other);

    public bool Equals(BoundingBox other) => Min == other.Min && Max == other.Max;

    public override int GetHashCode() => HashCode.Combine(Min, Max);

    public override string ToString() => $"BoundingBox (Min: {Min}, Max: {Max})";

    public static BoundingBox operator +(BoundingBox bounds, Vector2 value)
    {
        return new BoundingBox(bounds.Min + value, bounds.Max + value);
    }

    public static BoundingBox operator -(BoundingBox bounds, Vector2 value)
    {
        return new BoundingBox(bounds.Min - value, bounds.Max - value);
    }

    public static BoundingBox operator *(BoundingBox bounds, Vector2 value)
    {
        return new BoundingBox(bounds.Min * value, bounds.Max * value);
    }

    public static BoundingBox operator /(BoundingBox bounds, Vector2 value)
    {
        return new BoundingBox(bounds.Min / value, bounds.Max / value);
    }

    public static bool operator ==(BoundingBox left, BoundingBox right) => left.Equals(right);

    public static bool operator !=(BoundingBox left, BoundingBox right) => !(left == right);
}
