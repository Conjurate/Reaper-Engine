using System.Runtime.CompilerServices;

namespace Reaper;

public static class Log
{
    public static void Info(object message)
    {
        Console.WriteLine(message);
    }

    public static void Warn(object message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING] {message}");
        Console.ResetColor();
    }

    public static void Error(object message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {message}");
        Console.ResetColor();
    }

    public static void Debug(object message, [CallerMemberName] string caller = "")
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"[DEBUG] [{caller}] {message}");
        Console.ResetColor();
    }
}
