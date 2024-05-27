namespace Reaper;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequireModuleAttribute(params Type[] types) : Attribute
{
    public Type[] ModuleTypes { get; } = types;
}
