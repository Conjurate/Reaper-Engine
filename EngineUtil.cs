using System;
using System.Diagnostics;
using System.Numerics;

namespace Reaper;

public static class EngineUtil
{
    #region Game

    /// <summary>
    /// Ticks the timer towards the target. Once the timer is complete, 
    /// the timer is set to timerReset if specified.
    /// Call in Update()
    /// </summary>
    public static bool Tick(ref float timer, float target, float? timerReset = null)
    {
        if (timer == target)
        {
            timer = timerReset ?? timer;
            return timer != target;
        }

        bool increase = timer < target;
        timer += increase ? Time.Delta : -Time.Delta;

        bool complete = increase ? timer >= target : timer <= target;
        if (complete)
            timer = timerReset ?? target;

        return complete;
    }

    #endregion

    #region List

    public static IList<T> Swap<T>(IList<T> list, int a, int b)
    {
        (list[b], list[a]) = (list[a], list[b]);
        return list;
    }

    public static List<T> QuickSort<T>(List<T> list, int leftIndex, int rightIndex, Comparison<T> compare)
    {
        if (list.Count <= 1 || leftIndex >= rightIndex) 
            return list;

        int left = leftIndex;
        int right = rightIndex;
        T pivot = list[left];

        while (left <= right)
        {
            while (compare.Invoke(list[left], pivot) < 0)
            {
                left++;
            }

            while (compare.Invoke(list[right], pivot) > 0)
            {
                right--;
            }

            if (left <= right)
            {
                (list[right], list[left]) = (list[left], list[right]);
                left++;
                right--;
            }
        }

        if (leftIndex < right)
            QuickSort(list, leftIndex, right, compare);

        if (left < rightIndex)
            QuickSort(list, left, rightIndex, compare);

        return list;
    }

    #endregion List

    #region Math

    public static int Range(int value, int min, int max)
    {
        return value < min ? min : value > max ? max : value;
    }

    public static Vector2 Rotate(Vector2 vector, float angle) // Radians
    {
        return new Vector2(vector.X * MathF.Cos(angle) - vector.Y * MathF.Sin(angle), vector.X * MathF.Sin(angle) + vector.Y * MathF.Cos(angle));
    }

    #endregion Math

    #region Vector

    public static Vector2 GetDirTowards(Vector2 sourcePos, Vector2 targetPos, float multiplier = 1)
    {
        return Vector2.Normalize((targetPos - sourcePos)) * multiplier;
    }

    public static float GetRotTowards(Vector2 source, Vector2 dest)
    {
        float dx = dest.X - source.X;
        float dy = dest.Y - source.Y;
        return (float)Math.Atan2(dy, dx);
    }

    public static float Get360RotTowards(Vector2 source, Vector2 dest)
    {
        float dx = dest.X - source.X;
        float dy = dest.Y - source.Y;

        float angleRadians = (float)Math.Atan2(dy, dx);
        float angleDegrees = angleRadians * (180.0f / (float)Math.PI);

        return MathF.Abs((angleDegrees - 450) % 360);
    }

    #endregion Vector
}
