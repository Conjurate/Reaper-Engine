using System.Reflection;

namespace Reaper;

/// <summary>
/// Used to retrieve a module's dependencies and priority.
/// </summary>
internal static class ModuleCache
{
    private static readonly Dictionary<Type, HashSet<Type>> dependencies = [];
    private static Dictionary<Type, int> priorities = [];

    public static void Build()
    {
        // Build cache
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                // Build required modules
                IEnumerable<RequireModuleAttribute> reqModules = type.GetCustomAttributes<RequireModuleAttribute>();
                if (reqModules != null)
                {
                    HashSet<Type> types = [];
                    foreach (RequireModuleAttribute att in reqModules)
                    {
                        foreach (Type modType in att.ModuleTypes)
                            types.Add(modType);
                    }
                    dependencies[type] = types;
                }

                // Set module priority
                ModulePriorityAttribute priority = type.GetCustomAttribute<ModulePriorityAttribute>();
                if (priority != null)
                    priorities[type] = priority.Priority;
            }
        }
    }

    public static bool Requires(Type type, params Type[] types)
    {
        if (dependencies.TryGetValue(type, out HashSet<Type> result))
        {
            foreach (Type t in types)
            {
                if (!result.Contains(t))
                    return false;
            }
            return true;
        }

        return false;
    }

    public static HashSet<Type> GetDependencies(Type type)
    {
        if (dependencies.TryGetValue(type, out HashSet<Type> result))
            return new HashSet<Type>(result);
        else
            return [];
    }

    public static int GetPriority(Type type) => priorities.GetValueOrDefault(type, 0);
}
