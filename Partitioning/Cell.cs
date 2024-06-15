using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reaper;

public class Cell : IEnumerable<Entity>
{
    public int X => x;
    public int Y => y;
    public int Count => entities.Count;
    internal double RemovalTime;

    private int x;
    private int y;
    private HashSet<Entity> entities = [];

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Add(Entity entity)
    {
        return entities.Add(entity);
    }

    public bool Remove(Entity entity)
    {
        return entities.Remove(entity);
    }

    public void Update()
    {
        foreach (Entity entity in entities)
        {
            entity.Update();
        }
    }

    public IEnumerator<Entity> GetEnumerator()
    {
        return entities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
