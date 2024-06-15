using Raylib_cs;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace Reaper.UI;

[RequireModule(typeof(RectTransform))]
public class Label : EntityModule, ICanvasRenderable
{
    public Font Font
    {
        get => font;
        set => font = value;
    }
    
    public float FontSize
    {
        get => fontSize;
        set => fontSize = value;
    }
    
    public float Spacing
    {
        get => spacing;
        set => spacing = value;
    }
    
    public string Text
    {
        get => text;
        set => text = value;
    }

    public Color Tint { get; set; } = Color.White;

    public float Rotation { get; set; }

    private Font font;
    private float fontSize;
    private float spacing;
    private string text;

    public Label(string text, Font font, int fontSize)
    {
        this.text = text;
        this.font = font;
        this.fontSize = fontSize;
        spacing = 1.0f;
    }

    public void Render(RenderMode mode)
    {
        Vector2 pos = Transform.Position;

        if (mode == RenderMode.World)
        {
            pos *= Engine.PixelsPerUnit;
            pos.Y *= -1;
        }

        Raylib.DrawTextPro(Font.raylibFont, Text, pos, Vector2.Zero, Rotation, FontSize, Spacing, Tint.raylibColor);
    }

    public Rectangle MeasureRect()
    {
        Vector2 textSize = Raylib.MeasureTextEx(font.raylibFont, text, fontSize, spacing);
        return new Rectangle(0, 0, textSize.X, textSize.Y);
    }
}
