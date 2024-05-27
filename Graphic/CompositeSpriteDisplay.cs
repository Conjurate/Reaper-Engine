using System.Numerics;
using static System.Formats.Asn1.AsnWriter;

namespace Reaper;

public class CompositeSpriteDisplay : EntityModule, IRenderableWorld, IRenderableShader
{
    public int WorldLayer { get; set; }
    public Shader? Shader { get; set; }

    public bool Relative { get; set; } = true; // Offset positions are relative to the body
    public Color Tint { get; set; } = Color.White;
    public bool Flipped { get; set; }
    public float Rot { get; set; }
    public float Scale { get; set; } = 1.0f;
    public int Size => sortedParts.Count;

    private List<SpritePart> sortedParts = [];
    private Dictionary<string, SpritePart> parts = [];

    public CompositeSpriteDisplay(string baseName, Sprite baseSprite, int layer = 0)
    {
        Add(baseName, Relative ? Vector2.Zero : Position, baseSprite, layer);
    }

    public SpritePart Get(string name) => parts[name];

    public SpritePart Add(string name, Vector2 offset, Sprite sprite = null, int layer = 1)
    {
        Remove(name);
        SpritePart part = new SpritePart(sprite, offset, layer);
        sortedParts.Add(part);
        parts[name] = part;
        Util.QuickSort(sortedParts, 0, sortedParts.Count - 1, (a, b) => a.Layer.CompareTo(b.Layer));
        return part;
    }

    public SpritePart Remove(string name)
    {
        if (parts.TryGetValue(name, out var part)) 
        {
            sortedParts.Remove(part);
            parts.Remove(name);
            return part;
        }

        return null;
    }

    public bool ModifyPart(string name, Action<SpritePart> action)
    {
        if (parts.TryGetValue(name, out SpritePart part))
        {
            action.Invoke(part);
            return true;
        }

        return false;
    }

    public bool HasSprite(string name)
    {
        if (parts.TryGetValue(name, out SpritePart part))
        {
            return part.Sprite != null;
        }

        return false;
    }

    public bool IsRenderable(RenderMode mode)
    {
        if (!Visible) return false;

        foreach (SpritePart part in sortedParts)
        {
            if (part.Sprite == null) 
                continue;

            float x = ((Relative ? X : 0) + part.Offset.X) - (part.Sprite.Texture.Width / Engine.PixelsPerUnit * part.Sprite.Pivot.X);
            float y = ((Relative ? Y : 0) + part.Offset.Y) - (part.Sprite.Texture.Height / Engine.PixelsPerUnit * part.Sprite.Pivot.Y);

            if (Camera.Bounds.Intersects(x, y, x + part.Sprite.Texture.Width / Engine.PixelsPerUnit, y + part.Sprite.Texture.Height / Engine.PixelsPerUnit))
                return true;
        }

        return false;
    }

    public void Render(RenderMode mode)
    {
        foreach (SpritePart part in sortedParts)
        {
            if (part.Sprite == null) 
                continue;

            Vector2 renderPosition = Relative ? Position + part.Offset : part.Offset;
            Engine.DrawSprite(part.Sprite, renderPosition, Tint, Rot, Scale);
        }
    }
}

public class SpritePart
{
    public Sprite Sprite { get; set; }
    public Vector2 Offset { get; set; }
    public int Layer { get; }

    public SpritePart(Sprite sprite, Vector2 offset, int layer)
    {
        Sprite = sprite;
        Offset = offset;
        Layer = layer;
    }
}
