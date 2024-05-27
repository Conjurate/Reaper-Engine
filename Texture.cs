namespace Reaper;

public struct Texture
{
    public int Width => raylibTexture.Width;
    public int Height => raylibTexture.Height;

    internal Raylib_cs.Texture2D raylibTexture;

    internal Texture(Raylib_cs.Texture2D texture)
    {
        raylibTexture = texture;
    }
}
