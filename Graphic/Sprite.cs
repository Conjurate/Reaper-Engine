using Raylib_cs;
using System.Numerics;

namespace Reaper;

public class Sprite
{
    public Texture Texture { get; private set; }
    public Vector2 Pivot { get; set; }

    public Sprite(Texture texture, Vector2 pivot = default)
    {
        Texture = texture;
        Pivot = pivot;
    }
}
