using System.Numerics;

namespace Reaper;

public class EngineObject
{
    public int Id { get; private set; } = Engine.GetNextId();
    public Vector2 Position
    {
        get => position;
        set
        {
            Vector2 oldPos = position;
            position = value;
            PositionChanged?.Invoke(this, oldPos);
        }
    }
    public float X
    {
        get => position.X;
        set
        {
            Vector2 oldPos = position;
            position.X = value;
            PositionChanged?.Invoke(this, oldPos);
        }
    }
    public float Y
    {
        get => position.Y;
        set
        {
            Vector2 oldPos = position;
            position.Y = value;
            PositionChanged?.Invoke(this, oldPos);
        }
    }
    public bool Active { get; set; }
    public bool Visible { get; set; } = true;

    internal Action<EngineObject, Vector2> PositionChanged; // prevPos

    private Vector2 position;
}
