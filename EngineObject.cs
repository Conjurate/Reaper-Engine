using System.Numerics;

namespace Reaper;

public class EngineObject()
{
    public int Id { get; private set; } = Engine.EntityIds.NextId;
    public bool Active { get; set; }
}
