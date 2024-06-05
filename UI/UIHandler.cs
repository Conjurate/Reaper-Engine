namespace Reaper.UI;

public class UIHandler
{
    private List<Canvas> canvases = [];

    public void AddCanvases(Entity entity)
    {
        List<Canvas> canvasList = entity.GetModules<Canvas>();
        if (canvasList.Count > 0)
        {
            foreach (Canvas canvas in canvasList)
                canvases.Add(canvas);

            Log.Debug($"Registered canvas from entity {entity.Id} ({entity.GetType()}) ({canvases.Count})");
        }

        entity.ModuleStateChanged += ModuleStateChanged;
    }

    public void RemoveCanvases(Entity entity)
    {
        List<Canvas> canvasList = entity.GetModules<Canvas>();
        if (canvasList.Count > 0)
        {
            canvases.RemoveAll(c => c.Owner == entity);
            Log.Debug($"Unregistered all canvases from entity {entity.Id} ({entity.GetType()}) ({canvases.Count})");
        }

        entity.ModuleStateChanged -= ModuleStateChanged;
    }

    private static Comparison<Canvas> comparison = (a, b) =>
    {
        if (a.Mode == b.Mode)
        {
            return a.Layer.CompareTo(b.Layer);
        }
        else
        {
            return a.Mode.CompareTo(b.Mode);
        }
    };

    public void Update()
    {
        EngineUtil.QuickSort(canvases, 0, canvases.Count - 1, comparison);

        foreach (Canvas canvas in canvases)
        {
            // Interact with first canvas
            if (canvas.CheckInteract())
                return;
        }
    }

    private void ModuleStateChanged(Entity entity, EntityModule module, bool added)
    {
        if (module is Canvas canvas)
        {
            if (added)
            {
                canvases.Add(canvas);
                Log.Debug($"Registered canvas from entity {entity.Id} ({entity.GetType()}) ({canvases.Count})");
            }
            else if (canvases.Remove(canvas))
            {
                Log.Debug($"Unregistered canvas from entity {entity.Id} ({entity.GetType()}) ({canvases.Count})");
            }
        }
    }
}
