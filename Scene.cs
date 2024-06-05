using Raylib_cs;
using Reaper.UI;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Reaper;

// TODO: Create a list for cameras on the scene. Sort the cameras and use the first one.
public class Scene
{
    private static Camera2D debugCamera = new Camera2D(Vector2.Zero, new Vector2(-Screen.Width / 2.0f, -Screen.Height / 2.0f), 0f, 1f);

    public string Name => name;
    public Camera Camera { get; set; }

    private string name;
    internal Camera2D camera2D = new Camera2D(Vector2.Zero, Vector2.Zero, 0f, 1f);
    private Queue<Entity> spawnQueue = [];
    private Queue<Entity> deleteQueue = [];
    private Dictionary<int, Entity> objects = [];
    private GraphicRenderer graphics = new GraphicRenderer();
    private UIHandler ui = new UIHandler();

    public Scene(string name)
    {
        this.name = name;
        Entity camEntity = new Entity("Camera");
        Camera camera = new Camera();
        camEntity.AddModule(camera);
        Spawn(camEntity);
        Camera = camera;
        UpdateCamera();
    }

    public void Load()
    {
        ProcessQueues(true);

        UpdateCamera();

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

        ProcessQueues(true);
    }

    public void Spawn(params Entity[] entities)
    {
        foreach (Entity entity in entities)
        {
            if (entity.Scene != null)
            {
                Log.Error($"Attempted to spawn already spawned entity {entity.Id} in scene {entity.Scene.Name}");
                continue;
            }
            entity.Scene = this;
            spawnQueue.Enqueue(entity);
        }
    }

    public void Remove(params Entity[] entities)
    {
        foreach (Entity entity in entities)
        {
            entity.Scene = null;
            deleteQueue.Enqueue(entity);
        }
    }

    private void UpdateCamera()
    {
        Camera.UpdateBounds();
        camera2D.Target = Camera.Transform.Position;
        camera2D.Offset = Camera.Offset;
        camera2D.Zoom = Camera.Zoom;
        camera2D.Rotation = Camera.Transform.Rotation;
    }

    private void SpawnEntity(Entity entity, List<Entity> spawned, bool loading)
    {
        if (objects.ContainsKey(entity.Id))
        {
            Log.Error($"Attempted to spawn an already spawned entity. (ID: {entity.Id}, Type: {entity.GetType()})");
            return;
        }

        objects.Add(entity.Id, entity);
        graphics.AddRenderables(entity);
        ui.AddCanvases(entity);

        if (!loading)
        {
            spawned ??= [];
            spawned.Add(entity);
        }

        // Spawn children
        foreach (Transform child in entity.Transform.Children)
        {
            SpawnEntity(child.Owner, spawned, loading);
        }
    }

    private void RemoveEntity(Entity entity)
    {
        objects.Remove(entity.Id);
        graphics.RemoveRenderables(entity);
        ui.RemoveCanvases(entity);

        // Remove children
        foreach (Transform child in entity.Transform.Children)
        {
            RemoveEntity(child.Owner);
        }
    }

    private void ProcessQueues(bool loading)
    {
        // Delete queued entities
        while (deleteQueue.Count > 0)
        {
            Entity entity = deleteQueue.Dequeue();
            RemoveEntity(entity);
        }

        List<Entity> spawned = null;

        // Spawn queued entities
        while (spawnQueue.Count > 0)
        {
            Entity entity = spawnQueue.Dequeue();
            SpawnEntity(entity, spawned, loading);
        }

        // Initialize and load all new entities
        if (spawned != null)
        {
            foreach (Entity entity in spawned)
            {
                if (!entity.Initialized)
                    entity.Init();
            }

            spawned.ForEach(e => e.Load());
        }
    }

    public List<Entity> GetEntities()
    {
        return [.. objects.Values];
    }

    public List<T> Find<T>()
    {
        List<T> values = [];

        foreach(Entity entity in objects.Values)
        {
            List<T> list = entity.GetModules<T>();
            values.AddRange(list);
        }

        return values;
    }

    public void Update()
    {
        ProcessQueues(false);

        // Update entities
        foreach (Entity obj in objects.Values)
        {
            obj.Update();
        }

        // Update camera
        if (Camera.Updated)
        {
            UpdateCamera();
        }

        // Update UI
        ui.Update();

        // Render graphics
        Render();
    }

    private void Render()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib_cs.Color.Black);

        // World rendering
        Raylib.BeginMode2D(Engine.Debug ? debugCamera : camera2D);
        graphics.Render(RenderMode.World);
        Raylib.EndMode2D();

        // Screen rendering
        graphics.Render(RenderMode.Screen);
        Raylib.EndDrawing();

        /*
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

        /*Raylib.DrawTextureRec(Engine.TargetRender.Texture, new Rectangle(0f, 0f, Engine.TargetRender.Texture.Width, -Engine.TargetRender.Texture.Height), 
            Vector2.Zero, Raylib_cs.Color.White);

        Rectangle source = new Rectangle(0, 0, Screen.TargetWidth, -Screen.TargetHeight); // - Because OpenGL coordinates are inverted
        Rectangle dest = new Rectangle (0, 0, Screen.Width, Screen.Height);
        //Rectangle dest = new Rectangle(-Screen.TargetScale, -Screen.TargetScale, Screen.Width + (Screen.TargetScale * 2), Screen.Height + (Screen.TargetScale * 2));

        Raylib.DrawTexturePro(Engine.TargetRender.Texture, source, dest, Vector2.Zero, 0f, Raylib_cs.Color.White);

        Raylib.EndDrawing();*/
    }

    public Entity this[int id] => objects.GetValueOrDefault(id, null);
}
