using System.Numerics;
using System.Reflection;

namespace Reaper;

public abstract class EntityModule : IIdentifiable, IEquatable<EntityModule>
{
    public int Id { get; private set; } = Engine.GetNextId();

    public Entity Owner => owner;
    public bool IsLoaded { get; private set; }

    private Entity owner;
    private Dictionary<string, Action> methodCache = [];

    internal void SetOwner(Entity owner) => this.owner = owner;

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

    public Vector2 Position
    {
        get => owner.Position;
        set => owner.Position = value;
    }
    public float X
    {
        get => owner.X;
        set => owner.X = value;
    }
    public float Y
    {
        get => owner.Y;
        set => owner.Y = value;
    }
    public bool Visible
    {
        get => owner.Visible;
        set => owner.Visible = value;
    }

    protected Camera Camera => SceneManager.ActiveScene.Camera;

    public T GetModule<T>(int index = 0) where T : EntityModule => owner.GetModule<T>(index);

    public List<T> GetModules<T>() where T : EntityModule => owner.GetModules<T>();

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
