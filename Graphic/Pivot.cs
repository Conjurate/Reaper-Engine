using System.Numerics;

namespace Reaper;

public static class Pivot
{
    public static readonly Vector2 TopLeft = new Vector2(0f, 1.0f);
    public static readonly Vector2 TopRight = new Vector2(1.0f, 1.0f);
    public static readonly Vector2 TopCenter = new Vector2(0.5f, 1.0f);
    public static readonly Vector2 Center = new Vector2(0.5f, 0.5f);
    public static readonly Vector2 BottomLeft = new Vector2(0f, 0f);
    public static readonly Vector2 BottomRight = new Vector2(1.0f, 0f);
    public static readonly Vector2 BottomCenter = new Vector2(0.5f, 0f);
}
