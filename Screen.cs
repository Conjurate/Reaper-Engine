using Raylib_cs;
using System.Numerics;

namespace Reaper;

public static class Screen
{
    internal static int PrevWidth = Width;
    internal static int PrevHeight = Height;

    // Screen
    public static int Width => Raylib.GetScreenWidth();
    public static int Height => Raylib.GetScreenHeight();
    public static Vector2 Center => new Vector2(Width / 2.0f, Height / 2.0f);
    public static bool IsResized { get; internal set; }
    public static BoundingBox Bounds => new BoundingBox(0, 0, Width, Height);

    // Target screen
    public static int TargetWidth => 1920;
    public static int TargetHeight => 1080;
    public static float TargetWidthRatio => (float)Width / TargetWidth;
    public static float TargetHeightRatio => (float)Height / TargetHeight;
    public static float TargetScale => MathF.Min((float)Width / TargetWidth, (float)Height / TargetHeight);
}
