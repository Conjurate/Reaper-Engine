using System.Numerics;

namespace Reaper;

public class Transform : EntityModule
{
    public Vector2 Position
    {
        get => parent == null ? localPosition : parent.Position + localPosition;
        set
        {
            Vector2 prevPos = Position;
            localPosition = parent == null ? value : value - parent.Position;
            UpdatedPosition?.Invoke(this, prevPos);
        }
    }

    public Vector2 LocalPosition
    {
        get => localPosition;
        set
        {
            Vector2 prevPos = Position;
            localPosition = value;
            UpdatedPosition?.Invoke(this, prevPos); // Notify with global position
        }
    }

    public float Scale
    {
        get => scale;
        set => scale = value;
    }

    public float Rotation
    {
        get => rotation;
        set => rotation = value;
    }

    public Transform Parent
    {
        get => parent;
        set
        {
            if (parent == value || value == this || (parent != null && parent.Owner.Scene != Owner.Scene))
                return;

            parent?.children.Remove(this);

            parent = value;

            if (parent != null && !parent.children.Contains(this))
            {
                parent.Owner.Scene?.Spawn(Owner);
                parent.children.Add(this);
            }
        }
    }

    public Transform Root => Parent == null ? this : Parent.Root;

    public IReadOnlyList<Transform> Children => children;

    public int ChildCount => children.Count;

    public event Action<Transform, Vector2> UpdatedPosition; // prev pos

    private Vector2 localPosition;
    private float scale;
    private float rotation;
    private Transform parent;
    private List<Transform> children = [];

    public void Move(float x, float y)
    {
        Vector2 pos = Position;
        Position = pos + new Vector2(x, y);
    }

    public bool IsChildOf(Transform parent) => Parent == parent;

    public Transform GetChild(int index) => children[index];

    public Transform Find(string name) => children.Find(t => t.Owner.Name == name);

    public List<T> Find<T>()
    {
        List<T> values = [];

        values.AddRange(Owner.GetModules<T>());

        foreach(Transform child in children)
        {
            values.AddRange(child.Owner.GetModules<T>());
        }

        return values;
    }

    public void ClearChildren()
    {
        List<Transform> childrenCopy = new List<Transform>(children);
        foreach (Transform child in childrenCopy)
        {
            child.Parent = null;
        }
    }

    #region Sibling

    public int GetSiblingIndex() => Parent == null ? -1 : Parent.children.IndexOf(this);

    public void SetSiblingIndex(int index)
    {
        if (Parent == null)
            return;

        int curIndex = GetSiblingIndex();
        EngineUtil.Swap(Parent.children, curIndex, index);
    }

    public void SetFirstSibling() => SetSiblingIndex(0);

    public void SetLastSibling()
    {
        if (Parent == null)
            return;

        SetSiblingIndex(Parent.children.Count - 1);
    }

    #endregion Sibling
}
