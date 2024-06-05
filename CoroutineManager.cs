using System;
using System.Collections;

namespace Reaper;

public static class CoroutineManager
{
    private static int nextId = 1;
    private static HashSet<int> coroutineIds = [];
    private static Dictionary<int, Coroutine> coroutines = [];

    public static int Start(IEnumerator routine)
    {
        int id = nextId++;
        coroutines[id] = new Coroutine(id, routine);
        coroutineIds.Add(id);
        Log.Debug($"Started coroutine {id} {routine.GetType()}");
        return id;
    }

    public static void Stop(int id)
    {
        if (coroutines.Remove(id))
        {
            coroutineIds.Remove(id);
        }
    }

    internal static void Update()
    {
        List<int> finished = null;
        foreach (int id in coroutineIds)
        {
            Stack<IEnumerator> stack = coroutines[id].Routine;

            if (stack.Count == 0)
            {
                coroutines.Remove(id);
                finished ??= [];
                finished.Add(id);
                Log.Debug($"Finished coroutine {id}");
                continue;
            }

            IEnumerator routine = stack.Peek();
            if (!routine.MoveNext())
            {
                stack.Pop();
                continue;
            }

            if (routine.Current is IEnumerator next && routine != next)
                stack.Push(next);
        }

        if (finished != null)
        {
            foreach (int id in finished)
            {
                coroutineIds.Remove(id);
            }
        }
    }
}

public class Coroutine
{
    public int Id { get; private set; }
    public Stack<IEnumerator> Routine { get; private set; }

    public Coroutine(int id, IEnumerator routine)
    {
        this.Id = id;
        Routine = new Stack<IEnumerator>([routine]);
    }
}

public class WaitForSeconds : IEnumerator
{
    private double waitTime;
    private double startTime;

    public WaitForSeconds(float seconds)
    {
        waitTime = seconds;
        startTime = Time.Elapsed;
    }

    public bool MoveNext()
    {
        return (Time.Elapsed - startTime) < waitTime;
    }

    public void Reset() { }

    public object Current => null;
}
