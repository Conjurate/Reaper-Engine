using Raylib_cs;

namespace Reaper;

public class Time
{
    /// <summary>
    /// Time in seconds for last frame drawn
    /// </summary>
    public static float Delta => Raylib.GetFrameTime();

    /// <summary>
    /// Time elapsed since start of program
    /// </summary>
    public static double Elapsed => Raylib.GetTime();

    /// <summary>
    /// Current FPS
    /// </summary>
    public static int FPS => Raylib.GetFPS();
}
