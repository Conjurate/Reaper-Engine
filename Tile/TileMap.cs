using System.Numerics;

namespace Reaper;

public class TileMap : EntityModule, IRenderableWorld, IRenderableShader
{
    public int Layer { get; set; }
    public Shader? Shader { get; set; }
    public SpriteSheet TileSheet => tileSheet;
    public int TileSize => tileSize;

    private SpriteSheet tileSheet;
    private int tileSize;
    private int width;
    private int height;
    private int[,] tiles;

    public TileMap(SpriteSheet tileSheet, int tileSize, int width, int height, int layer = -1)
    {
        this.tileSheet = tileSheet;
        this.tileSize = tileSize;
        this.width = width;
        this.height = height;
        tiles = new int[width, height];
        Layer = layer;
    }

    public void SetTile(int x, int y, int id) => tiles[x, y] = id;

    public void Fill(int id)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = id;
            }
        }
    }

    public int GetTileId(int x, int y) => tiles[x, y];

    private void Update()
    {
        // Update tiles
        // animatedTiles.ForEach(tile => tile.Animate());
    }

    public bool IsRenderable(RenderMode mode) => true;

    public void Render(RenderMode mode)
    {
        BoundingBox bounds = Camera.Bounds;

        // Clamp to the tile array limits
        int minX = Math.Max((int)Math.Floor(bounds.Min.X - Transform.Position.X), 0);
        int maxX = Math.Min((int)Math.Ceiling(bounds.Max.X - Transform.Position.X), tiles.GetLength(0));
        int minY = Math.Max((int)Math.Floor(bounds.Min.Y - Transform.Position.Y), 0);
        int maxY = Math.Min((int)Math.Ceiling(bounds.Max.Y - Transform.Position.Y), tiles.GetLength(1));

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                int id = tiles[x, y];

                if (id < 0 || id >= TileSheet.Length) 
                    continue;

                Vector2 pos = Transform.Position + new Vector2(x, y);

                Engine.DrawSprite(TileSheet[id], pos, Color.White);
            }
        }
    }
}
