using Raylib_cs;
using System.Numerics;

namespace Reaper;

public static class Mouse
{
    public static Vector2 WorldPosition 
    {
        get
        {
            Vector2 worldPos = Raylib.GetScreenToWorld2D(Position, SceneManager.ActiveScene.camera2D);
            worldPos.Y *= -1;
            return worldPos / Engine.PixelsPerUnit;
        }
    }
    public static Vector2 Position => Raylib.GetMousePosition();
    //public static Vector2 ScaledPosition => Raylib.GetMousePosition() * new Vector2((float)Screen.TargetWidth / Screen.Width, (float)Screen.TargetHeight / Screen.Height);
    public static int X => Raylib.GetMouseX();
    public static int Y => Raylib.GetMouseY();

    public static float AngleFromCenter
    {
        get 
        {
            Vector2 screenCenter = Screen.Center;
            Vector2 mousePosition = Raylib.GetMousePosition();

            float dx = mousePosition.X - screenCenter.X;
            float dy = mousePosition.Y - screenCenter.Y;

            float angleRadians = (float)Math.Atan2(dy, dx);

            return angleRadians * (180.0f / (float)Math.PI);
        }
    }
}
