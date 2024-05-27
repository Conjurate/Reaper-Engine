namespace Reaper;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ModulePriorityAttribute(int priority) : Attribute
{
    public int Priority { get; } = priority;
}
