using Raylib_cs;
using System.Diagnostics;
using System.Numerics;

namespace Reaper;

public class SpriteDisplay : EntityModule, IRenderableWorld, IRenderableShader
{
    public Sprite Sprite { get; set; }

    public int Layer { get; set; }
    public Shader? Shader { get; set; }
    public Color Tint { get; set; } = Color.White;
    public bool Flipped { get; set; }
    public float Rot { get; set; }
    public float Scale { get; set; } = 1.0f;

    public SpriteDisplay(Sprite sprite)
    {
        Sprite = sprite;
    }

    public bool IsRenderable(RenderMode mode) 
    {
        float width = Sprite.Texture.Width * Engine.Pixel;
        float height = Sprite.Texture.Height * Engine.Pixel;

        float x = Transform.Position.X - (width * Sprite.Pivot.X);
        float y = Transform.Position.Y - (height * Sprite.Pivot.Y);

        return Camera.Bounds.Intersects(x, y, x + width, y + height);
    }

    public void Render(RenderMode mode)
    {
        Engine.DrawSprite(Sprite, Transform.Position, Tint, Rot, Scale);

        if (!Engine.Debug) return;

        float x = Transform.Position.X - ((Sprite.Texture.Width * Sprite.Pivot.X) * Engine.Pixel);
        float y = Transform.Position.Y - ((Sprite.Texture.Height * Sprite.Pivot.Y) * Engine.Pixel);

        //Engine.DrawRectangle(x, y, Sprite.Texture.Width, Sprite.Texture.Height, Color.Red);
        Engine.DrawBounds(RenderMode.World, new BoundingBox(x, y, x + (Sprite.Texture.Width * Engine.Pixel), y + (Sprite.Texture.Height * Engine.Pixel)), Color.Red);

        //Engine.DrawRectangle((int)xPixel, (int)yPixel, Sprite.Texture.Width, Sprite.Texture.Height, Color.Blue);
    }
}
