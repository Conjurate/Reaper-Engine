using Raylib_cs;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace Reaper.UI;

public class Label() : UIElement
{
    public Font Font
    {
        get => font;
        set
        {
            font = value;
            UpdateBounds();
        }
    }
    public float FontSize
    {
        get => fontSize;
        set
        {
            fontSize = value;
            UpdateBounds();
        }
    }
    public float Spacing
    {
        get => spacing;
        set
        {
            spacing = value;
            UpdateBounds();
        }
    }
    public string Text
    {
        get => text;
        set
        {
            text = value;
            UpdateBounds();
        }
    }
    public Color Tint { get; set; } = Color.White;
    public float Rotation { get; set; }

    private Font font;
    private float fontSize;
    private float spacing;
    private string text;

    public Label(string text, Font font, int fontSize) : this()
    {
        Text = text;
        Font = font;
        FontSize = fontSize;
        spacing = 1.0f;
        UpdateBounds();
    }

    public override void Render(RenderMode mode)
    {
        Vector2 pos = Position;

        if (mode == RenderMode.World)
        {
            pos *= Engine.PixelsPerUnit;
            pos.Y *= -1;
        }

        Raylib.DrawTextPro(Font.raylibFont, Text, pos, Vector2.Zero, Rotation, FontSize, Spacing, Tint.raylibColor);
    }

    private void UpdateBounds()
    {
        Vector2 textSize = Raylib.MeasureTextEx(font.raylibFont, text, fontSize, spacing);
        scaledBounds = new BoundingBox(0, 0, textSize.X, textSize.Y);
    }
}
