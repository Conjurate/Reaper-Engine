using Raylib_cs;
using System.ComponentModel;
using System.Numerics;
using static System.Formats.Asn1.AsnWriter;

namespace Reaper;

public class Scene
{
    private static Camera2D debugCamera = new Camera2D(Vector2.Zero, new Vector2(-Screen.Width / 2.0f, -Screen.Height / 2.0f), 0f, 1f);

    public string Name => name;
    public Camera Camera { get; set; } = new Camera();

    private string name;
    private Dictionary<int, Entity> objects = [];
    private GraphicRenderer graphics = new GraphicRenderer();

    public Scene(string name)
    {
        this.name = name;
        Spawn(Camera);
    }

    public void Load()
    {
        foreach (Entity entity in objects.Values)
        {
            if (!entity.Initialized)
                entity.Init();
        }

        foreach (Entity entity in objects.Values)
        {
            entity.Load();
        }
    }

    public void Unload()
    {
        foreach (Entity entity in objects.Values)
        {
            entity.Unload();
        }
    }

    public void Spawn(params Entity[] entities)
    {
        foreach (Entity entity in entities)
        {
            if (objects.ContainsKey(entity.Id))
            {
                Log.Error($"Attempted to spawn an already spawned entity. (ID: {entity.Id}, Type: {entity.GetType()})");
                continue;
            }
            objects.Add(entity.Id, entity);
            graphics.AddRenderables(entity);
        }
    }

    public void Remove(params Entity[] entities)
    {
        foreach (Entity entity in entities)
        {
            objects.Remove(entity.Id);
            graphics.RemoveRenderables(entity);
        }
    }

    public void Update()
    {
        foreach (Entity obj in objects.Values)
        {
            obj.Update();
        }

        Render();
    }

    private void Render()
    {
        Raylib.BeginTextureMode(Engine.TargetRender);

        Raylib.ClearBackground(Raylib_cs.Color.Black);

        // World rendering
        Raylib.BeginMode2D(Engine.Debug ? debugCamera : Camera.camera2D);
        graphics.Render(RenderMode.World);
        Raylib.EndMode2D();

        // Screen rendering
        graphics.Render(RenderMode.Screen);

        Raylib.EndTextureMode();

        // Draw target render
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib_cs.Color.Black);
        // Draw render texture to screen, properly scaled
        Raylib.DrawTextureRec(Engine.TargetRender.Texture, new Rectangle(0f, 0f, Engine.TargetRender.Texture.Width, -Engine.TargetRender.Texture.Height), 
            Vector2.Zero, Raylib_cs.Color.White);
        Raylib.EndDrawing();
    }

    public Entity this[int id] => objects.GetValueOrDefault(id, null);
}
