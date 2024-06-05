using Raylib_cs;
using Reaper.UI;
using System.Numerics;
using System.Runtime.InteropServices;
using Rectangle = Reaper.UI.Rectangle;

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
    public static bool Initialized => init;

    private static int ppu = 16;
    private static float pixel = 1.0f / ppu;
    private static bool init;

    #region Id

    internal static Identity EntityIds = new Identity();

    #endregion Id

    public static void Init(int width, int height, string title, int targetFPS, int flags = 0)
    {
        ModuleCache.Build();
        Raylib.SetConfigFlags((ConfigFlags)flags);
        Raylib.InitWindow(width, height, title);
        Raylib.SetTargetFPS(targetFPS);
        Raylib.InitAudioDevice();
        Raylib.SetExitKey(KeyboardKey.Null);
        init = true;
    }

    public static void Run()
    {
        if (!init)
        {
            Log.Error("You must call Engine.Init before running.");
            return;
        }

        while (!Raylib.WindowShouldClose())
        {
            int width = Screen.Width;
            int height  = Screen.Height;
            if (width != Screen.PrevWidth || height != Screen.PrevHeight)
            {
                Screen.IsResized = true;
                Screen.PrevWidth = width;
                Screen.PrevHeight = height;
            }
            CoroutineManager.Update();
            SceneManager.Update();
            Screen.IsResized = false;
        }

        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }

    public static void Quit()
    {
        System.Environment.Exit(1);
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

        float width = texture.Width * scale;
        float height = texture.Height * scale;

        Raylib_cs.Rectangle sourceRec = new Raylib_cs.Rectangle(0, 0, texture.Width, texture.Height);
        Raylib_cs.Rectangle destRec = new Raylib_cs.Rectangle(drawPos.X + width / 2.0f, drawPos.Y + height / 2.0f, width, height);
        Vector2 origin = new Vector2(width / 2.0f, height / 2.0f);

        // Draw the texture
        Raylib.DrawTexturePro(texture.raylibTexture, sourceRec, destRec, origin, rot, tint.raylibColor);
    }

    /// <summary>
    /// Draw a texture to the screen.
    /// </summary>
    public static void DrawTexture(Texture texture, Vector2 pos, Rectangle dest, Color tint, float rot = 0, float scale = 1.0f)
    {
        Vector2 drawPos = pos;
        drawPos.Y *= -1;
        drawPos.Y -= texture.Height;

        float width = texture.Width * scale;
        float height = texture.Height * scale;

        Raylib_cs.Rectangle sourceRec = new Raylib_cs.Rectangle(0, 0, texture.Width, texture.Height);
        Raylib_cs.Rectangle destRec = new Raylib_cs.Rectangle(dest.X + width / 2.0f, dest.Y + height / 2.0f, dest.Width, dest.Height);
        Vector2 origin = new Vector2(width / 2.0f, height / 2.0f);

        // Draw the texture
        Raylib.DrawTexturePro(texture.raylibTexture, sourceRec, destRec, origin, rot, tint.raylibColor);
    }

    /// <summary>
    /// Draw a rectangle to the screen.
    /// </summary>
    public static void DrawRectangle(float x, float y, float width, float height, Color color, bool filled = false)
    {
        if (filled)
            Raylib.DrawRectanglePro(new Raylib_cs.Rectangle(x, -y-height, width, height), Vector2.Zero, 0, color.raylibColor);
        else
            Raylib.DrawRectangleLinesEx(new Raylib_cs.Rectangle(x, -y-height, width, height), 1.0f, color.raylibColor);
    }

    /// <summary>
    /// Draw a rectangle to the screen.
    /// </summary>
    public static void DrawRectangle(Rectangle rect, Color color, bool filled = false)
    {
        if (filled)
            Raylib.DrawRectanglePro(new Raylib_cs.Rectangle(rect.X, -rect.Y - rect.Height, rect.Width, rect.Height), Vector2.Zero, 0, color.raylibColor);
        else
            Raylib.DrawRectangleLinesEx(new Raylib_cs.Rectangle(rect.X, -rect.Y - rect.Height, rect.Width, rect.Height), 1.0f, color.raylibColor);
    }

    /// <summary>
    /// Draw a sprite to world space using the engine's coordinate system.
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
    public static void DrawBounds(RenderMode mode, BoundingBox box, Color color, bool filled = false)
    {
        float x = box.Min.X;
        float y = box.Min.Y;
        float width = box.Width;
        float height = box.Height;
        if (mode == RenderMode.Screen)
        {
            y *= -1;
            y -= box.Height;
        } else
        {
            x *= PixelsPerUnit;
            y *= PixelsPerUnit;
            width *= PixelsPerUnit;
            height *= PixelsPerUnit;
        }

        DrawRectangle(x, y, width, height, color, filled);
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

    public static SpriteSheet LoadSpriteSheet(string path, int width, int height, Vector2 pivot = default)
    {
        Raylib_cs.Image image = Raylib.LoadImage(path);
        SpriteSheet sheet = new SpriteSheet(image, width, height, pivot);
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
        Raylib_cs.Font raylibFont = Raylib.LoadFontEx(path, fontSize, charRange, codepointCount);
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

    public static Sound LoadSound(string path)
    {
        return new Sound(Raylib.LoadSound(path));
    }

    public static void UnloadSound(Sound sound)
    {
        Raylib.UnloadSound(sound.raylibSound);
    }

    #endregion
}