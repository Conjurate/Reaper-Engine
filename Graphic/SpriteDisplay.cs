using Raylib_cs;
using System.Diagnostics;
using System.Numerics;

namespace Reaper;

public class SpriteDisplay(Sprite sprite) : EntityModule, IRenderableWorld, IRenderableShader
{
    public Sprite Sprite
    {
        get => sprite;
        set
        {
            sprite = value;
            screenWidth = (int)sprite.Texture.Width / Engine.PixelsPerUnit;
            screenHeight = (int)sprite.Texture.Height / Engine.PixelsPerUnit;
        }
    }

    public int WorldLayer { get; set; }
    public Shader? Shader { get; set; }
    public Color Tint { get; set; } = Color.White;
    public bool Flipped { get; set; }
    public float Rot { get; set; }
    public float Scale { get; set; } = 1.0f;

    private int screenWidth = (int)sprite.Texture.Width / Engine.PixelsPerUnit;
    private int screenHeight = (int)sprite.Texture.Height / Engine.PixelsPerUnit;

    public bool IsRenderable(RenderMode mode) 
    {
        if (!Visible) return false;

        float x = X - (screenWidth * Sprite.Pivot.X);
        float y = Y - (screenHeight * Sprite.Pivot.Y);

        return Camera.Bounds.Intersects(x, y, x + screenWidth, y + screenHeight);
    }

    public void Render(RenderMode mode)
    {
        Engine.DrawSprite(Sprite, Position, Tint, Rot, Scale);

        if (!Engine.Debug) return;

        float xF = X - (screenWidth * Sprite.Pivot.X);
        float yF = Y - (screenHeight * Sprite.Pivot.Y);

        int x = (int) (xF * Engine.PixelsPerUnit);
        int y = (int) (yF * Engine.PixelsPerUnit);

        Engine.DrawRectangle(x, y, sprite.Texture.Width, sprite.Texture.Height, Color.Blue);
    }
}
