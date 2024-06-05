using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Reaper.UI;

public struct Rectangle
{
    public float X 
    {
        get => raylibRect.X;
        set => raylibRect.X = value;
    }
    
    public float Y
    {
        get => raylibRect.Y;
        set => raylibRect.Y = value;
    }
    
    public float Width
    {
        get => raylibRect.Width;
        set => raylibRect.Width = value;
    }
    
    public float Height
    {
        get => raylibRect.Height;
        set => raylibRect.Height = value;
    }

    public Vector2 Position
    {
        readonly get => raylibRect.Position;
        set => raylibRect.Position = value;
    }

    public Vector2 Size
    {
        readonly get => raylibRect.Size;
        set => raylibRect.Size = value;
    }

    internal Raylib_cs.Rectangle raylibRect;

    public Rectangle(float x, float y, float width, float height)
    {
        raylibRect = new Raylib_cs.Rectangle(x, y, width, height);
    }

    public bool Contains(float x, float y) => X <= x && X + Width >= x && Y <= y && Y + Height >= y;

    public bool Contains(Vector2 pos) => Contains(pos.X, pos.Y);

    public override string ToString() => raylibRect.ToString();
}
