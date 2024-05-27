using Raylib_cs;
using System.Numerics;

namespace Reaper;

public class Camera : Entity
{
    internal Camera2D camera2D;

    public Vector2 Offset
    {
        get => camera2D.Offset;
        set
        {
            camera2D.Offset = value;
            UpdateBounds();
        }
    }
    public float Zoom
    {
        get => camera2D.Zoom;
        set
        {
            camera2D.Zoom = value;
            UpdateBounds();
        }
    }
    public float Rotation
    {
        get => camera2D.Rotation;
        set
        {
            camera2D.Rotation = value;
            //UpdateBounds();
        }
    }
    public BoundingBox Bounds => bounds;

    private BoundingBox bounds;

    public Camera()
    {
        Zoom = 1.0f;
        Offset = Screen.Center;
        UpdateBounds();
        PositionChanged += UpdatedPosition;
    }

    private void UpdateBounds()
    {
        float halfWidth = Screen.Width * 0.5f / camera2D.Zoom * Engine.Pixel;
        float halfHeight = Screen.Height * 0.5f / camera2D.Zoom * Engine.Pixel;
        bounds = new BoundingBox(X - halfWidth, Y - halfHeight, X + halfWidth, Y + halfHeight);
    }

    private void UpdatedPosition(EngineObject entity, Vector2 oldPos)
    {
        camera2D.Target = Position * Engine.PixelsPerUnit;
        camera2D.Target.Y *= -1;
        UpdateBounds();
    }
}
