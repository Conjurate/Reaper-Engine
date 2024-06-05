using Raylib_cs;
using System.Collections;
using System.Numerics;

namespace Reaper;

public class SpriteSheet : IEnumerable<Sprite>
{
    public int SpriteWidth => width;
    public int SpriteHeight => height;
    public int Length => sprites.Length;

    private int width;
    private int height;
    private Sprite[] sprites;

    public SpriteSheet(Image source, int width, int height, Vector2 pivot = default)
    {
        this.width = width;
        this.height = height;
        int columns = source.Width / width;
        int rows = source.Height / height;
        sprites = new Sprite[columns * rows];

        // Load sprites
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Rectangle sourceRect = new Rectangle(col * width, row * height, width, height);
                Image srcCopy = Raylib.ImageCopy(source);

                // Crop out texture and convert to Sprite
                Raylib.ImageCrop(ref srcCopy, new Rectangle((int)sourceRect.X, (int)sourceRect.Y, (int)sourceRect.Width, (int)sourceRect.Height));
                Texture2D texture = Raylib.LoadTextureFromImage(srcCopy);
                Sprite sprite = new Sprite(new Texture(texture), pivot);

                // Place into sprites and unload copied image
                int index = (row * columns) + col;
                sprites[index] = sprite;
                Raylib.UnloadImage(srcCopy);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Sprite> GetEnumerator()
    {
        foreach (Sprite sprite in sprites)
            yield return sprite;
    }

    public Sprite this[int index] => sprites[index];
}
