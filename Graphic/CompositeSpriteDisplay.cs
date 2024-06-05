using Raylib_cs;
using System.Collections;
using System.Numerics;
using static System.Formats.Asn1.AsnWriter;

namespace Reaper;

public class CompositeSpriteDisplay : EntityModule, IEnumerable<SpritePart>, IRenderableWorld, IRenderableShader
{
    public int Layer { get; set; }
    public Shader? Shader { get; set; }

    public bool Relative { get; set; } = true; // Offset positions are relative to the body
    public bool Flipped { get; set; }
    public float Rot { get; set; }
    public float Scale { get; set; } = 1.0f;
    public int Count => sortedParts.Count;

    private List<SpritePart> sortedParts = [];
    private Dictionary<string, SpritePart> parts = [];

    public CompositeSpriteDisplay()
    {

    }

    public CompositeSpriteDisplay(object baseName, Sprite baseSprite, int layer = 0)
    {
        Add(baseName, Relative ? Vector2.Zero : Transform.Position, baseSprite, layer);
    }

    public SpritePart Get(object name) => parts.GetValueOrDefault(name.ToString(), null);

    public SpritePart Add(object name, Vector2 offset, Sprite sprite = null, int layer = 1)
    {
        Remove(name);
        SpritePart part = new SpritePart(sprite, offset, layer);
        sortedParts.Add(part);
        parts[name.ToString()] = part;
        EngineUtil.QuickSort(sortedParts, 0, sortedParts.Count - 1, (a, b) => a.Layer.CompareTo(b.Layer));
        return part;
    }

    public SpritePart Remove(object name)
    {
        if (parts.TryGetValue(name.ToString(), out var part)) 
        {
            sortedParts.Remove(part);
            parts.Remove(name.ToString());
            return part;
        }

        return null;
    }

    public void Clear()
    {
        sortedParts.Clear();
        parts.Clear();
    }

    public bool ModifyPart(object name, Action<SpritePart> action)
    {
        if (parts.TryGetValue(name.ToString(), out SpritePart part))
        {
            action.Invoke(part);
            return true;
        }

        return false;
    }

    public bool HasSprite(object name)
    {
        if (parts.TryGetValue(name.ToString(), out SpritePart part))
        {
            return part.Sprite != null;
        }

        return false;
    }

    public bool IsRenderable(RenderMode mode)
    {
        foreach (SpritePart part in sortedParts)
        {
            if (part.Sprite == null) 
                continue;

            float x = ((Relative ? Transform.Position.X : 0) + part.Position.X) - (part.Sprite.Texture.Width / Engine.PixelsPerUnit * part.Sprite.Pivot.X);
            float y = ((Relative ? Transform.Position.Y : 0) + part.Position.Y) - (part.Sprite.Texture.Height / Engine.PixelsPerUnit * part.Sprite.Pivot.Y);

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

            Vector2 renderPosition = Relative ? Transform.Position + part.Position : part.Position;

            if (part.Shader != null)
                Raylib.BeginShaderMode(part.Shader.Value.raylibShader);

            Engine.DrawSprite(part.Sprite, renderPosition, part.Tint, Rot + part.Rot, part.Scale * Scale);

            if (part.Shader != null)
                Raylib.EndShaderMode();

            if (Engine.Debug)
            {
                float x = part.Position.X - ((part.Sprite.Texture.Width * part.Sprite.Pivot.X) * Engine.Pixel);
                float y = part.Position.Y - ((part.Sprite.Texture.Height * part.Sprite.Pivot.Y) * Engine.Pixel);
                Engine.DrawBounds(RenderMode.World, 
                    new BoundingBox(x, y, x + (part.Sprite.Texture.Width * Engine.Pixel), y + (part.Sprite.Texture.Height * Engine.Pixel)), Color.Red);
            }
        }
    }

    public IEnumerator<SpritePart> GetEnumerator() => sortedParts.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => sortedParts.GetEnumerator();

    public SpritePart this[object key] => Get(key);
}

public class SpritePart : IRenderableShader
{
    public Sprite Sprite { get; set; }
    public int Layer { get; }
    public Shader? Shader { get; set; }
    public Vector2 Position { get; set; }
    public Color Tint { get; set; } = Color.White;
    public float Rot { get; set; }
    public float Scale { get; set; } = 1.0f;

    public SpritePart(Sprite sprite, Vector2 position, int layer = 0)
    {
        Sprite = sprite;
        Position = position;
        Layer = layer;
    }
}
