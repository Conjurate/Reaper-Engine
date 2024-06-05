using Raylib_cs;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Reaper;

public class Camera : EntityModule
{
    public Vector2 Offset { get; set; } = Screen.Center;
    public float Zoom { get; set; } = 1.0f;
    public BoundingBox Bounds => bounds;
    public bool Updated
    {
        get
        {
            if (prevPos != Transform.Position) return true;
            if (prevOffset != Offset) return true;
            if (prevRot != Transform.Rotation) return true;
            if (prevZoom != Zoom) return true;
            return false;
        }
    }

    private BoundingBox bounds;
    private Vector2 prevPos;
    private Vector2 prevOffset;
    private float prevZoom;
    private float prevRot;

    public Camera()
    {

    }

    private void Update()
    {
        if (Screen.IsResized)
        {
            Offset = Screen.Center;
        }
    }

    internal void UpdateBounds()
    {
        float halfWidth = (Screen.Width * Engine.Pixel / Zoom) * 0.5f;
        float halfHeight = (Screen.Height * Engine.Pixel / Zoom) * 0.5f;
        bounds = new BoundingBox(Transform.Position.X - halfWidth, Transform.Position.Y - halfHeight,
            Transform.Position.X + halfWidth, Transform.Position.Y + halfHeight);

        prevPos = Transform.Position;
        prevOffset = Offset;
        prevZoom = Zoom;
        prevRot = Transform.Rotation;
    }

    private void UpdatedPosition(Transform transform, Vector2 oldPos)
    {
        //camera2D.Target = transform.Position * Engine.PixelsPerUnit;
        //camera2D.Target.Y *= -1;
        UpdateBounds();
    }
}
