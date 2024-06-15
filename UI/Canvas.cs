using Raylib_cs;
using Reaper.UI;
using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Xml.Linq;

namespace Reaper;

[RequireModule(typeof(RectTransform))]
public class Canvas : EntityModule, IRenderableScreen, IRenderableWorld
{
    public int Layer { get; set; }
    public RenderMode Mode { get; set; } = RenderMode.Screen;

    private RectTransform rectTransform;

    private void Init()
    {
        rectTransform = Transform as RectTransform;
    }

    internal bool CheckInteract()
    {
        if (Input.IsMousePressed(InputKey.Mouse0))
        {
            Vector2 mousePos = Mode == RenderMode.Screen ? Mouse.Position : Mouse.WorldPosition;
            foreach (Transform trans in Owner.Transform.Children)
            {
                if (trans is RectTransform rectTransform)
                {
                    if (rectTransform.Rect.Contains(mousePos))
                    {
                        bool clicked = false;
                        foreach (IClickable clickable in trans.Find<IClickable>())
                        {
                            clickable.Click();
                            clicked = true;
                        }
                        if (clicked)
                            return true;
                    }
                }
            }
        }

        return false;
    }

    public bool IsRenderable(RenderMode mode)
    {
        if (Mode != mode) return false;
        return mode == RenderMode.Screen || SceneManager.ActiveScene.Camera.Bounds.Intersects(rectTransform.Rect);
    }

    public void Render(RenderMode mode)
    {
        foreach (Transform trans in Owner.Transform.Children)
        {
            foreach (ICanvasRenderable renderable in trans.Owner.GetModules<ICanvasRenderable>())
            {
                renderable.Render(mode);
            }
        }

        //Engine.DrawRectangle(rectTransform.Rect, Color.Blue);
    }
}
