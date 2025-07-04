﻿using Raylib_cs;
using System.Numerics;

namespace Reaper;

// Note: The transform object could change by adding a UI element to an entity
internal class GraphicRenderer
{
    private static Identity idGenerator = new Identity();

    internal List<ScreenRenderable> screen = [];
    internal List<WorldRenderable> world = [];
    private bool needsScreenSorting;
    private bool needsWorldSorting;

    public void ForceUpdate()
    {
        if (!Engine.YSort) return;
        needsScreenSorting = true;
        needsWorldSorting = true;
    }

    public int IndexOf(RenderMode mode, Entity entity)
    {
        return mode == RenderMode.Screen ? screen.FindIndex(0, r => r.Entity == entity) : 
            world.FindIndex(0, r => r.Entity == entity);
    }

    public void AddRenderables(Entity entity)
    {
        // Screen
        List<IRenderableScreen> screenRends = entity.GetModules<IRenderableScreen>();
        if (screenRends.Count > 0)
        {
            foreach (IRenderableScreen screenRend in screenRends)
                screen.Add(new ScreenRenderable(idGenerator.NextId, entity, screenRend));

            needsScreenSorting = Engine.YSort;
            Log.Debug($"Added screen renderer for entity {entity.Name} ({entity.GetType()})");
        }

        // World
        List<IRenderableWorld> worldRends = entity.GetModules<IRenderableWorld>();
        if (worldRends.Count > 0)
        {
            foreach (IRenderableWorld worldRend in worldRends)
                world.Add(new WorldRenderable(idGenerator.NextId, entity, worldRend));

            needsWorldSorting = Engine.YSort;
            Log.Debug($"Added world renderer for entity {entity.Name} ({entity.GetType()})");
        }

        entity.Transform.UpdatedPosition += PositionChanged;
        entity.ModuleStateChanged += ModuleStateChanged;
        Log.Debug($"Registered entity {entity.Name} to GraphicRenderer ({screen.Count}, {world.Count})");
    }

    public void RemoveRenderables(Entity entity)
    {
        // Screen
        List<IRenderableScreen> screenRends = entity.GetModules<IRenderableScreen>();
        if (screenRends.Count > 0)
        {
            screen.RemoveAll(t => t.Entity == entity);
        }

        // World
        List<IRenderableWorld> worldRends = entity.GetModules<IRenderableWorld>();
        if (worldRends.Count > 0)
        {
            world.RemoveAll(t => t.Entity == entity);
        }

        // Unregister
        entity.Transform.UpdatedPosition -= PositionChanged;
        entity.ModuleStateChanged -= ModuleStateChanged;
        Log.Debug($"Unregistered entity {entity.Name} from GraphicRenderer ({screen.Count}, {world.Count})");
    }

    public void Render(RenderMode mode)
    {
        switch (mode)
        {
            case RenderMode.Screen:
                {
                    if (needsScreenSorting)
                    {
                        EngineUtil.QuickSort(screen, 0, screen.Count - 1, (a, b) => {
                            // Check layers
                            int layerComparison = a.Renderable.Layer.CompareTo(b.Renderable.Layer);
                            if (layerComparison != 0) return layerComparison;

                            // Check entity id
                            int entityComparison = a.Entity.Id.CompareTo(b.Entity.Id);
                            if (entityComparison != 0) return entityComparison;

                            // Check render id
                            return a.Id.CompareTo(b.Id);
                        });
                        needsScreenSorting = false;
                    }

                    foreach (ScreenRenderable screenRend in screen)
                    {
                        if (screenRend.Renderable.IsRenderable(RenderMode.Screen))
                        {
                            IRenderableShader renderableShader = screenRend.Renderable as IRenderableShader;
                            if (renderableShader != null && renderableShader.Shader != null)
                            {
                                Raylib.BeginShaderMode(renderableShader.Shader.Value.raylibShader);
                            }

                            screenRend.Renderable.Render(RenderMode.Screen);

                            if (renderableShader != null && renderableShader.Shader != null)
                            {
                                Raylib.EndShaderMode();
                            }
                        }
                    }

                    break;
                }
            case RenderMode.World:
                {
                    if (needsWorldSorting)
                    {
                        EngineUtil.QuickSort(world, 0, world.Count - 1, (a, b) => {
                            // Check layers
                            int layerComparison = a.Renderable.Layer.CompareTo(b.Renderable.Layer);
                            if (layerComparison != 0) return layerComparison;

                            // Check y level
                            if (Engine.YSort)
                            {
                                int yComparison = b.Entity.Transform.Position.Y.CompareTo(a.Entity.Transform.Position.Y);
                                if (yComparison != 0) return yComparison;
                            }

                            // Check entity id
                            int entityComparison = a.Entity.Id.CompareTo(b.Entity.Id);
                            if (entityComparison != 0) return entityComparison;

                            // Check render id
                            return a.Id.CompareTo(b.Id);
                        });
                        needsWorldSorting = false;
                    }

                    foreach (WorldRenderable worldRend in world)
                    {
                        if (worldRend.Renderable.IsRenderable(RenderMode.World))
                        {
                            IRenderableShader renderableShader = worldRend.Renderable as IRenderableShader;
                            if (renderableShader != null && renderableShader.Shader != null)
                            {
                                Raylib.BeginShaderMode(renderableShader.Shader.Value.raylibShader);
                            }

                            worldRend.Renderable.Render(RenderMode.World);

                            if (renderableShader != null && renderableShader.Shader != null)
                            {
                                Raylib.EndShaderMode();
                            }
                        }
                    }

                    break;
                }
            default: break;
        }
    }

    private void PositionChanged(Transform transform, Vector2 oldPos)
    {
        if (Engine.YSort && transform.Position.Y != oldPos.Y)
        {
            needsWorldSorting = true;
        }
    }

    private void ModuleStateChanged(Entity entity, EntityModule module, bool added)
    {
        if (module is IRenderableScreen screenRend)
        {
            if (added)
            {
                screen.Add(new ScreenRenderable(idGenerator.NextId, entity, screenRend));
                needsScreenSorting = true;
                Log.Debug($"Added screen renderer for entity {entity.Name} ({entity.GetType()})");
            }
            else
            {
                screen.RemoveAll(sr => sr.Entity == entity && sr.Renderable == screenRend);
            }
        }

        if (module is IRenderableWorld worldRend)
        {
            if (added)
            {
                world.Add(new WorldRenderable(idGenerator.NextId, entity, worldRend));
                needsWorldSorting = true;
                Log.Debug($"Added world renderer for entity {entity.Name} ({entity.GetType()})");
            }
            else
            {
                world.RemoveAll(wr => wr.Entity == entity && wr.Renderable == worldRend);
            }
        }
    }
}

public enum RenderMode
{
    Screen,
    World
}

public struct ScreenRenderable(int id, Entity entity, IRenderableScreen renderable)
{
    public int Id => id;
    public Entity Entity => entity;
    public IRenderableScreen Renderable => renderable;
}

public struct WorldRenderable(int id, Entity entity, IRenderableWorld renderable)
{
    public int Id => id;
    public Entity Entity => entity;
    public IRenderableWorld Renderable => renderable;
}
