using Raylib_cs;
using System.Numerics;

namespace Reaper;

public class Engine
{
    public static bool Debug { get; set; }
    public static int PixelsPerUnit
    {
        get => ppu;
        set
        {
            ppu = value;
            pixel = 1.0f / ppu;
        }
    }
    public static float Pixel => pixel;

    public static float TargetScale { get; private set; }
    public static int TargetWidth { get; private set; }
    public static int TargetHeight { get; private set; }

    private static int ppu = 16;
    private static float pixel = 1.0f / ppu;
    internal static RenderTexture2D TargetRender;
    //private static Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0f, 1.0f);

    #region Id

    private static int idCounter;

    public static int GetNextId() => ++idCounter;

    #endregion Id

    public static void Init(int width, int height, string title, int targetFPS, int targetWidth = 1920, int targetHeight = 1080)
    {
        ModuleCache.Build();
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(width, height, title);
        Raylib.SetTargetFPS(targetFPS);
        TargetWidth = targetWidth;
        TargetHeight = targetHeight;
    }

    public static void Run(int targetWidth, int targetHeight)
    {
        TargetRender = Raylib.LoadRenderTexture(TargetWidth, TargetHeight);
        Raylib.SetTextureFilter(TargetRender.Texture, TextureFilter.Point);

        while (!Raylib.WindowShouldClose())
        {
            TargetScale = MathF.Min((float)Screen.Width / TargetWidth, (float)Screen.Height / TargetHeight);
            SceneManager.Update();
        }

        Raylib.UnloadRenderTexture(TargetRender);

        Raylib.CloseWindow();
    }

    #region Graphics

    /// <summary>
    /// Draw a pixel to the screen.
    /// </summary>
    public static void DrawPixel(int x, int y, Color color)
    {
        Raylib.DrawPixel(x, -y, color.raylibColor);
    }

    /// <summary>
    /// Draw a line to the screen.
    /// </summary>
    public static void DrawLine(int startX, int startY, int endX, int endY, Color color)
    {
        Raylib.DrawLine(startX, -startY, endX, -endY, color.raylibColor);
    }

    /// <summary>
    /// Draw a line to the screen.
    /// </summary>
    public static void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 1.0f)
    {
        Vector2 startPos = start;
        Vector2 endPos = end;
        startPos.Y *= -1;
        endPos.Y *= -1;
        Raylib.DrawLineEx(startPos, endPos, thickness, color.raylibColor);
    }

    /// <summary>
    /// Draw a texture to the screen.
    /// </summary>
    public static void DrawTexture(Texture texture, int x, int y, Color color)
    {
        Raylib.DrawTexture(texture.raylibTexture, x, -y-texture.Height, color.raylibColor);
    }

    /// <summary>
    /// Draw a texture to the screen.
    /// </summary>
    public static void DrawTexture(Texture texture, Vector2 pos, Color tint, float rot = 0, float scale = 1.0f)
    {
        Vector2 drawPos = pos;
        drawPos.Y *= -1;
        drawPos.Y -= texture.Height;
        Raylib.DrawTextureEx(texture.raylibTexture, drawPos, rot, scale, tint.raylibColor);
    }

    /// <summary>
    /// Draw a rectangle to the screen.
    /// </summary>
    public static void DrawRectangle(int x, int y, int width, int height, Color color, bool filled = false)
    {
        if (filled)
            Raylib.DrawRectangle(x, -y-height, width, height, color.raylibColor);
        else
            Raylib.DrawRectangleLines(x, -y-height, width, height, color.raylibColor);
    }

    /// <summary>
    /// Draw a sprite to world space.
    /// </summary>
    public static void DrawSprite(Sprite sprite, Vector2 pos, Color tint, float rot = 0, float scale = 1.0f)
    {
        Vector2 drawPos = pos;
        drawPos *= Engine.PixelsPerUnit;
        drawPos.X -= (sprite.Texture.Width * sprite.Pivot.X);
        drawPos.Y -= (sprite.Texture.Height * sprite.Pivot.Y);
        DrawTexture(sprite.Texture, drawPos, tint, rot, scale);
    }

    /// <summary>
    /// Draw a bounds using a render mode.
    /// </summary>
    public static void DrawBounds(RenderMode mode, BoundingBox box, float xOffset = 0, float yOffset = 0, bool filled = false)
    {
        int x = (int)(box.Min.X + xOffset);
        int y = mode == RenderMode.Screen ? -(int)(box.Min.Y + yOffset) - (int)box.Height : (int)(box.Min.Y + yOffset);
        Engine.DrawRectangle(x, y, (int)box.Width, (int)box.Height, Color.Blue, filled);
    }

    #endregion Graphics

    #region Loading

    public static Texture LoadTexture(string path)
    {
        Texture2D texture = Raylib.LoadTexture(path);
        return new Texture(texture);
    }

    public static void UnloadTexture(Texture texture)
    {
        Raylib.UnloadTexture(texture.raylibTexture);
    }

    public static Sprite LoadSprite(string path, Vector2 pivot = default)
    {
        Texture texture = LoadTexture(path);
        return new Sprite(texture, pivot);
    }

    public static SpriteSheet LoadSpriteSheet(string path, int width, int height)
    {
        Image image = Raylib.LoadImage(path);
        SpriteSheet sheet = new SpriteSheet(image, width, height);
        Raylib.UnloadImage(image);
        return sheet;
    }

    public static void UnloadSpriteSheet(SpriteSheet sheet)
    {
        foreach (Sprite sprite in sheet)
        {
            UnloadTexture(sprite.Texture);
        }
    }

    public static UI.Font LoadFont(string path, int fontSize, int[] charRange = null, int codepointCount = 95)
    {
        Font raylibFont = Raylib.LoadFontEx(path, fontSize, charRange, codepointCount);
        Raylib.SetTextureFilter(raylibFont.Texture, TextureFilter.Point);
        return new UI.Font(raylibFont);
    }

    public static void UnloadFont(UI.Font font)
    {
        Raylib.UnloadFont(font.raylibFont);
    }

    public static Shader LoadShader(string vsPath, string fsPath)
    {
        return new Shader(Raylib.LoadShader(vsPath, fsPath));
    }

    public static void UnloadShader(Shader shader)
    {
        Raylib.UnloadShader(shader.raylibShader);
    }

    #endregion
}