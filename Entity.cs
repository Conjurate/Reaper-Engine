using System;
using System.Numerics;

namespace Reaper;

public class Entity() : EngineObject, IIdentifiable, IEquatable<Entity>
{
    public bool HasModules => modules != null;

    private Dictionary<Type, List<EntityModule>> modules;
    private SortedList<int, List<EntityModule>> sortedModules;

    public Entity(float x, float y) : this()
    {
        X = x;
        Y = y;
    }

    #region Internal

    internal Action<Entity, EntityModule, bool> ModuleStateChanged; // true = added, false = removed
    internal bool Initialized { get; private set; }
    internal bool Spawned { get; set; }

    internal void Init()
    {
        // Verify dependencies
        if (modules != null)
        {
            // Verify dependencies
            Dictionary<Type, List<EntityModule>>.Enumerator enumerator = modules.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<Type, List<EntityModule>> entry = enumerator.Current;
                HashSet<Type> dependencies = ModuleCache.GetDependencies(entry.Key);
                foreach (Type type in dependencies)
                {
                    if (modules.ContainsKey(type))
                        continue;

                    modules.Remove(entry.Key);

                    int priority = ModuleCache.GetPriority(type);
                    List<EntityModule> prioModules = sortedModules[priority];
                    foreach (EntityModule module in modules[type])
                    {
                        if (prioModules.Remove(module))
                            Log.Info("Removed prio module " + priority);
                    }

                    Log.Warn($"Missing module dependency: {GetType().Name} does not have {type.Name} as a required module for {entry.Key.Name}");
                }
            }

            RunForModules(m => m.CallInit());
            Initialized = true;
        }
    }

    internal void Load()
    {
        RunForModules(m => m.CallLoad());
    }

    internal void Unload()
    {
        RunForModules(m => m.CallUnload());
    }

    internal void RunForModules(Action<EntityModule> action)
    {
        if (sortedModules != null)
        {
            foreach (KeyValuePair<int, List<EntityModule>> kvp in sortedModules)
            {
                foreach (EntityModule module in kvp.Value)
                {
                    action.Invoke(module);
                }
            }
        }
    }

    internal void Update() => RunForModules(m => m.CallUpdate());

    #endregion Internal

    #region Module

    public void AddModule<T>(T module) where T : EntityModule
    {
        Type type = typeof(T);
        modules ??= [];
        sortedModules ??= [];

        if (!modules.TryGetValue(type, out List<EntityModule> typeModules))
        {
            typeModules = [];
            modules[type] = typeModules;
        }

        module.SetOwner(this);
        typeModules.Add(module);

        // Module priority
        int priority = ModuleCache.GetPriority(type);
        if (!sortedModules.TryGetValue(priority, out List<EntityModule> value))
        {
            value = [];
            sortedModules[priority] = value;
        }
        value.Add(module);

        ModuleStateChanged?.Invoke(this, module, true);
    }

    public void RemoveModule<T>(T module) where T : EntityModule
    {
        Type type = typeof(T);

        // Remove module
        if (modules.TryGetValue(type, out List<EntityModule> typeModules))
            typeModules.Remove(module);

        // Remove module priority
        int priority = ModuleCache.GetPriority(type);
        List<EntityModule> prioModules = sortedModules[priority];
        prioModules.Remove(module);
        if (prioModules.Count == 0)
            sortedModules.Remove(priority);

        ModuleStateChanged?.Invoke(this, module, false);
    }

    public T GetModule<T>(int index = 0)
    {
        if (index < 0)
            return default;

        Type type = typeof(T);
        if (modules.TryGetValue(type, out List<EntityModule> typeModules) != true)
        {
            if (index == 0)
                return modules.Values.SelectMany(list => list).OfType<T>().FirstOrDefault();
            else
            {
                List<T> mods = GetModules<T>();
                return index >= mods.Count ? default : mods[index];
            }
        }

        if (index >= typeModules.Count)
            return default;

        return (T)Convert.ChangeType(typeModules[index], type);
    }

    public List<T> GetModules<T>()
    {
        if (modules == null)
            return [];

        Type type = typeof(T);
        if (modules.TryGetValue(type, out List<EntityModule> typeModules) != true)
        {
            return modules.Values.SelectMany(list => list).OfType<T>().ToList();
        }

        return typeModules.Cast<T>().ToList();
    }

    #endregion Module

    #region Hash and Equality

    public override int GetHashCode() => Id;

    public override bool Equals(object obj) => Equals(obj as Entity);

    public bool Equals(Entity other)
    {
        if (other is null)
            return false;

        return other.Id == Id;
    }

    #endregion Hash and Equality
}
