using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reaper;

public class Cell
{
    private int x;
    private int y;
    private HashSet<Entity> entities = [];

    public void Add(Entity entity)
    {
        entities.Add(entity);
    }

    public void Remove(Entity entity)
    {
        entities.Remove(entity);
    }

    public void Update()
    {
        foreach (Entity entity in entities)
        {
            entity.Update();
        }
    }
}
