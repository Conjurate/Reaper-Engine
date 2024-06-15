using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Reaper;

public class Grid : IEnumerable<Entity>
{
    private const int CleanDelay = 120;

    private Dictionary<(int, int), Cell> cells = [];
    private Dictionary<string, Entity> entityByName = [];
    private HashSet<(int, int)> removalQueue = [];

    internal void ProcessRemoval()
    {
        if (removalQueue.Count == 0) return;
        Log.Debug("Removal queue size: " + removalQueue.Count);
        foreach ((int, int) key in removalQueue)
        {
            cells.Remove(key);
            Log.Debug("Removed cell " + key);
        }
        removalQueue.Clear();
    }

    private (int, int) GetKey(Vector2 pos)
    {
        int x = (int)MathF.Floor(pos.X / Engine.CellSize);
        int y = (int)MathF.Floor(pos.Y / Engine.CellSize);
        return (x, y);
    }

    private bool Add(Entity entity, (int, int) key)
    {
        if (!cells.TryGetValue(key, out Cell cell))
        {
            cell = new Cell(key.Item1, key.Item2);
            cells[key] = cell;
        }

        removalQueue.Remove(key);
        bool added = cell.Add(entity);
        if (added)
        {
            //Log.Debug($"Added entity {entity.Name} to cell ({cell.X}, {cell.Y}) ({cell.Count})");
        }
        return added;
    }

    public void Add(Entity entity)
    {
        if (entityByName.ContainsKey(entity.Name))
            return;

        if (Add(entity, GetKey(entity.Transform.Position)))
        {
            entityByName.Add(entity.Name, entity);
            entity.Transform.UpdatedPosition += EntityMoved;
            Log.Debug($"Registered entity {entity.Name}");
        }
    }

    private bool Remove(Entity entity, (int, int) key, Cell cell)
    {
        bool removed = cell.Remove(entity);
        if (removed && cell.Count == 0)
        {
            removalQueue.Add(key);
            //cells.Remove(key);
            //Log.Debug($"Removed entity {entity.Name} from cell ({cell.X}, {cell.Y}) ({cell.Count})");
        }
        return removed;
    }

    public void Remove(Entity entity)
    {
        (int, int) key = GetKey(entity.Transform.Position);
        if (cells.TryGetValue(key, out Cell cell))
        {
            if (Remove(entity, key, cell))
            {
                entityByName.Remove(entity.Name);
                entity.Transform.UpdatedPosition -= EntityMoved;
                Log.Debug($"Unregistered entity {entity.Name}");
            }
        }
    }

    public bool Contains(Entity entity)
    {
        return entityByName.ContainsKey(entity.Name);
    }

    public List<Cell> QueryCells(Vector2 min, Vector2 max)
    {
        List<Cell> result = [];
        (int, int) minKey = GetKey(min);
        (int, int) maxKey = GetKey(max);

        for (int x = minKey.Item1; x <= maxKey.Item1; x++)
        {
            for (int y = minKey.Item2; y <= maxKey.Item2; y++)
            {
                if (cells.TryGetValue((x, y), out Cell value))
                {
                    result.Add(value);
                }
            }
        }

        return result;
    }

    public List<Entity> QueryEntities(Vector2 min, Vector2 max)
    {
        List<Entity> entities = [];
        (int, int) minKey = GetKey(min);
        (int, int) maxKey = GetKey(max);

        for (int x = minKey.Item1; x <= maxKey.Item1; x++)
        {
            for (int y = minKey.Item2; y <= maxKey.Item2; y++)
            {
                if (cells.TryGetValue((x, y), out Cell value))
                {
                    entities.AddRange(value);
                }
            }
        }

        return entities;
    }

    public List<Entity> QueryEntities(Vector2 source, float dist)
    {
        Vector2 offset = new Vector2(dist, dist);
        return QueryEntities(source - offset, source + offset);
    }

    private void EntityMoved(Transform transform, Vector2 prevPos)
    {
        (int, int) curKey = GetKey(transform.Position);
        (int, int) prevKey = GetKey(prevPos);

        if (curKey != prevKey)
        {
            // Remove from previous cell
            if (cells.TryGetValue(prevKey, out Cell cell))
            {
                Remove(transform.Owner, prevKey, cell);
            }

            // Add to new cell
            Add(transform.Owner, curKey);
        }
    }

    public IEnumerator<Entity> GetEnumerator()
    {
        return entityByName.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Entity this[string name] => entityByName.GetValueOrDefault(name, null);
}
