using Raylib_cs;
using System.Numerics;

namespace Reaper;

public static class Mouse
{
    public static Vector2 WorldPosition 
    {
        get
        {
            Vector2 worldPos = Raylib.GetScreenToWorld2D(Position, SceneManager.ActiveScene.Camera.camera2D);
            worldPos.Y *= -1;
            return worldPos / Engine.PixelsPerUnit;
        }
    }
    public static Vector2 Position => Raylib.GetMousePosition();
    public static int X => Raylib.GetMouseX();
    public static int Y => Raylib.GetMouseY();
}
