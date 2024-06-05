using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Reaper;

public static class Random
{
    private static readonly System.Random random = new System.Random();
    private static readonly object syncLock = new object();

    /// <summary>
    /// Generate an integer between an inclusive min
    /// and an exclusive max.
    /// </summary>
    public static int Range(int min, int max)
    {
        lock (syncLock)
        {
            return random.Next(min, max);
        }
    }

    /// <summary>
    /// Generate a double between an inclusive min
    /// and an exclusive max.
    /// </summary>
    public static double Range(double min, double max)
    {
        lock (syncLock)
        {
            return (random.NextDouble() * (max - min)) + min;
        }
    }

    /// <summary>
    /// Generate a float between an inclusive min
    /// and an exclusive max.
    /// </summary>
    public static float Range(float min, float max)
    {
        lock (syncLock)
        {
            return (float)((random.NextDouble() * (max - min)) + min);
        }
    }

    /// <summary>
    /// Generate a double that is greater than or
    /// equal to 0.0, or less than 1.0.
    /// </summary>
    public static double NextDouble()
    {
        lock (syncLock)
        {
            return random.NextDouble();
        }
    }

    /// <summary>
    /// Generate a float that is greater than or
    /// equal to 0.0, or less than 1.0.
    /// </summary>
    public static float NextFloat()
    {
        return (float)NextDouble();
    }

    /// <summary>
    /// Generate a random float value.
    /// </summary>
    /// <returns></returns>
    public static float GetFloat()
    {
        lock (syncLock)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            double exponent = Math.Pow(2.0, random.Next(-126, 128));
            return (float)(mantissa * exponent);
        }
    }

    /// <summary>
    /// Generate a random vector within a bounds.
    /// </summary>
    public static Vector2 PointInBounds(BoundingBox bounds)
    {
        float x = Range(bounds.Min.X, bounds.Max.X);
        float y = Range(bounds.Min.Y, bounds.Max.Y);
        return new Vector2(x, y);
    }

    /// <summary>
    /// Generate a random vector within a circle.
    /// </summary>
    public static Vector2 PointInCircle(float radius)
    {
        lock (syncLock)
        {
            double angle = random.NextDouble() * 2 * Math.PI;
            double r = radius * Math.Sqrt(random.NextDouble());

            float x = (float)(r * Math.Cos(angle));
            float y = (float)(r * Math.Sin(angle));

            return new Vector2(x, y);
        }
    }
}
