namespace Reaper;

public struct Color
{
    public static readonly Color White = new Color(255, 255, 255, 255);
    public static readonly Color Black = new Color(0, 0, 0, 255);
    public static readonly Color Red = new Color(255, 0, 0, 255);
    public static readonly Color Green = new Color(0, 255, 0, 255);
    public static readonly Color Blue = new Color(0, 0, 255, 255);

    public byte R { get => raylibColor.R; set => raylibColor.R = value; }
    public byte G { get => raylibColor.G; set => raylibColor.G = value; }
    public byte B { get => raylibColor.B; set => raylibColor.B = value; }
    public byte A { get => raylibColor.A; set => raylibColor.A = value; }

    internal Raylib_cs.Color raylibColor;

    public Color(int r, int g, int b, int a)
    {
        raylibColor = new Raylib_cs.Color(r, g, b, a);
    }
}
