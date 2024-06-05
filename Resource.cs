using System.Collections;

namespace Reaper;

public class Resource<TKey, T> : IEnumerable<T>
{
    private Dictionary<TKey, T> resources = [];

    public void Clear() => resources.Clear();

    public bool Remove(TKey key) => resources.Remove(key);

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var resource in resources.Values)
        {
            yield return resource;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T this[TKey key]
    {
        get => resources.GetValueOrDefault(key, default);
        set => resources[key] = value;
    }
}
