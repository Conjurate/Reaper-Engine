using Raylib_cs;
using System.Numerics;

namespace Reaper;

public static class Screen
{
    public static int Width => Raylib.GetScreenWidth();

    public static int Height => Raylib.GetScreenHeight();

    public static Vector2 Center => new Vector2(Width / 2.0f, Height / 2.0f);
}
