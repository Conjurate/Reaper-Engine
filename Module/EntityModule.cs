using System.Numerics;
using System.Reflection;

namespace Reaper;

public abstract class EntityModule : IIdentifiable, IEquatable<EntityModule>
{
    public int Id { get; private set; } = Engine.EntityIds.NextId;

    public Entity Owner { get; internal set; }
    public Transform Transform => Owner.Transform;
    public bool IsLoaded { get; private set; }

    private Dictionary<string, Action> methodCache = [];

    internal void CallInit() => CallMethod("Init", false);

    internal void CallLoad()
    {
        CallMethod("Load", true);
        IsLoaded = true;
    }

    internal void CallUnload()
    {
        CallMethod("Unload", true);
        IsLoaded = false;
    }

    internal void CallUpdate() => CallMethod("Update", true);
    internal void CallDelete() => CallMethod("Delete", false);

    private void CallMethod(string methodName, bool cache = false)
    {
        Action method = GetMethod(methodName, cache);
        method?.Invoke();
    }

    private Action GetMethod(string methodName, bool cache)
    {
        if (!methodCache.TryGetValue(methodName, out Action method) || !cache)
        {
            Type derivedType = GetType();
            MethodInfo methodInfo = derivedType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (methodInfo == null)
                return null;

            if (methodInfo.ReturnType != typeof(void) || methodInfo.GetParameters().Length != 0)
                return null;

            Action action = (Action)Delegate.CreateDelegate(typeof(Action), this, methodInfo);

            if (cache)
                methodCache[methodName] = action;
            
            method = action;
        }

        return method;
    }

    #region Helpers

    protected Camera Camera => SceneManager.ActiveScene.Camera;

    public T GetModule<T>(int index = 0) where T : EntityModule => Owner.GetModule<T>(index);

    public List<T> GetModules<T>() where T : EntityModule => Owner.GetModules<T>();

    public void Spawn(Entity entity) => SceneManager.ActiveScene.Spawn(entity);

    public void Remove(Entity entity) => SceneManager.ActiveScene.Remove(entity);

    #endregion Helpers

    #region Hash and Equality

    public override int GetHashCode() => Id;

    public override bool Equals(object obj) => Equals(obj as EntityModule);

    public bool Equals(EntityModule other)
    {
        if (other is null)
            return false;

        return other.Id == Id;
    }

    #endregion Hash and Equality
}
