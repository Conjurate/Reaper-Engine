using Reaper.UI;
using System.Diagnostics;
using System.Numerics;
using System.Xml.Linq;

namespace Reaper;

public class Canvas : EntityModule, IRenderableScreen, IRenderableWorld
{
    public int ScreenLayer { get; set; }
    public int WorldLayer { get; set; }
    public RenderMode RenderMode { get; set; } = RenderMode.Screen;
    public BoundingBox Bounds => scaledBounds.Bounds;

    private ScaledBounds scaledBounds;
    private List<UIElement> elements = [];

    public Canvas(RenderMode mode = RenderMode.Screen, float width = 1920, float height = 1080)
    {
        RenderMode = mode;
        scaledBounds = new ScaledBounds(width, height);
    }

    public void Add(UIElement element)
    {
        elements.Add(element);
    }

    public void Remove(UIElement element)
    {
        elements.Remove(element);
    }

    public void Resize(float width, float height)
    {
        scaledBounds = new ScaledBounds(width, height);
    }

    private void Update()
    {
        if (Input.IsMousePressed(InputKey.Mouse0))
        {
            Vector2 mousePos = RenderMode == RenderMode.Screen ? Mouse.Position : Mouse.WorldPosition;
            Log.Debug(mousePos);
            foreach (UIElement element in elements)
            {
                if (element.Bounds.Contains(mousePos))
                {
                    element.CallClicked();
                }
            }
        }
    }

    public bool IsRenderable(RenderMode mode)
    {
        if (RenderMode != mode) return false;
        return mode == RenderMode.Screen || SceneManager.ActiveScene.Camera.Bounds.Intersects(Bounds);
    }

    public void Render(RenderMode mode)
    {
        foreach (UIElement element in elements)
        {
            if (element.Visible)
            {
                element.Render(mode);
            }

            Engine.DrawBounds(RenderMode.Screen, element.Bounds);
        }

        Engine.DrawBounds(RenderMode.Screen, Bounds);
    }
}
