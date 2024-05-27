using Raylib_cs;
using System.Numerics;

namespace Reaper.UI;

public abstract class UIElement : EngineObject
{
    public event Action<UIElement> Clicked;
    public BoundingBox Bounds => scaledBounds + Position;

    protected BoundingBox scaledBounds;
    private BoundingBox targetBounds;
    private float aspectRatio;

    internal void CallClicked() => Clicked?.Invoke(this);

    public abstract void Render(RenderMode mode);

    public void Resize(float width, float height)
    {
        targetBounds = new BoundingBox(0, 0, width, height);
        aspectRatio = width / height;
        ScaleToFit(Screen.Width, Screen.Height);
    }

    public void ScaleToFit(float newWidth = -1, float newHeight = -1)
    {
        if (newWidth == -1)
            newWidth = Screen.Width;
        if (newHeight == -1)
            newHeight = Screen.Height;

        float newAspectRatio = newWidth / newHeight;

        if (newAspectRatio > aspectRatio)
        {
            // New screen is wider relative to the height, fit based on height
            float scale = newHeight / targetBounds.Height;
            scaledBounds = new BoundingBox(0, 0, targetBounds.Width * scale, targetBounds.Height * scale);
        }
        else
        {
            // New screen is taller relative to the width, fit based on width
            float scale = newWidth / targetBounds.Width;
            scaledBounds = new BoundingBox(0, 0, targetBounds.Width * scale, targetBounds.Height * scale);
        }
    }
}
