using Raylib_cs;
using System.Numerics;

namespace Reaper;

internal class GraphicRenderer
{
    private const bool YSort = true;

    private List<ScreenRenderable> screen = [];
    private List<WorldRenderable> world = [];
    private bool needsScreenSorting;
    private bool needsWorldSorting;

    public void AddRenderables(Entity entity)
    {
        // Screen
        List<IRenderableScreen> screenRends = entity.GetModules<IRenderableScreen>();
        if (screenRends.Count > 0)
        {
            foreach (IRenderableScreen screenRend in screenRends)
                screen.Add(new ScreenRenderable(entity, screenRend));

            needsScreenSorting = true;
            Log.Debug($"Added screen renderer for entity {entity.Id} ({entity.GetType()})");
        }

        // World
        List<IRenderableWorld> worldRends = entity.GetModules<IRenderableWorld>();
        if (worldRends.Count > 0)
        {
            foreach (IRenderableWorld worldRend in worldRends)
                world.Add(new WorldRenderable(entity, worldRend));

            needsWorldSorting = true;
            Log.Debug($"Added world renderer for entity {entity.Id} ({entity.GetType()})");
        }

        entity.PositionChanged += PositionChanged;
        entity.ModuleStateChanged += ModuleStateChanged;
        Log.Debug($"Registered entity {entity.Id} to GraphicRenderer ({screen.Count}, {world.Count})");
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
        entity.PositionChanged -= PositionChanged;
        entity.ModuleStateChanged -= ModuleStateChanged;
        Log.Debug($"Unregistered entity {entity.Id} from GraphicRenderer ({screen.Count}, {world.Count})");
    }

    public void Render(RenderMode mode)
    {
        switch (mode)
        {
            case RenderMode.Screen:
                {
                    if (needsScreenSorting)
                    {
                        Util.QuickSort(screen, 0, screen.Count - 1, (a, b) => a.Renderable.ScreenLayer.CompareTo(b.Renderable.ScreenLayer));
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
                        Util.QuickSort(world, 0, world.Count - 1, (a, b) => {
                            int layerComparison = a.Renderable.WorldLayer.CompareTo(b.Renderable.WorldLayer);
                            if (layerComparison != 0 || !YSort) return layerComparison;
                            return b.Entity.Y.CompareTo(a.Entity.Y);
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

    private void PositionChanged(EngineObject entity, Vector2 oldPos)
    {
        if (YSort && entity.Position.Y != oldPos.Y)
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
                screen.Add(new ScreenRenderable(entity, screenRend));
                needsScreenSorting = true;
                Log.Debug($"Added screen renderer for entity {entity.Id} ({entity.GetType()})");
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
                world.Add(new WorldRenderable(entity, worldRend));
                needsWorldSorting = true;
                Log.Debug($"Added world renderer for entity {entity.Id} ({entity.GetType()})");
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

public struct ScreenRenderable(Entity entity, IRenderableScreen renderable)
{
    public Entity Entity => entity;
    public IRenderableScreen Renderable => renderable;
}

public struct WorldRenderable(Entity entity, IRenderableWorld renderable)
{
    public Entity Entity => entity;
    public IRenderableWorld Renderable => renderable;
}
