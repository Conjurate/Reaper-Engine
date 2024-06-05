using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Reaper.UI;

[RequireModule(typeof(RectTransform))]
public class Image : EntityModule, IRenderableScreen, IRenderableWorld
{
    public int Layer { get; set; }
    public Texture Texture { get; set; }
    public Color Tint { get; set; } = Color.White;

    private RectTransform trans;

    public Image(Texture texture)
    {
        Texture = texture;
    }

    private void Init()
    {
        trans = Transform as RectTransform;
    }

    public bool IsRenderable(RenderMode mode) => true;

    public void Render(RenderMode mode)
    {
        Vector2 pos = Transform.Position;
        pos.Y *= -1;
        Engine.DrawTexture(Texture, pos, trans.Rect, Tint);
    }
}
