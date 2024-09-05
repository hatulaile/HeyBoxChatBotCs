using System.Reflection;
using HeyBoxBotCs.Api.Extensions;

namespace HeyBoxBotCs.Api.Features;

public static class Log
{
    public static HashSet<Assembly> DebugEnabled { get; } = new HashSet<Assembly>();

    private static object _consoleLock = new();

    public static void Info(object message)
    {
        Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", Enums.LogLevel.Info,
            Enums.LogLevel.Info.LogLevelTotalColor());
    }

    public static void Info(string message)
    {
        Send("[" + Assembly.GetCallingAssembly().GetName().Name + "] " + message, Enums.LogLevel.Info,
            Enums.LogLevel.Info.LogLevelTotalColor());
    }

    public static void Warn(object message)
    {
        Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", Enums.LogLevel.Warn,
            Enums.LogLevel.Warn.LogLevelTotalColor());
    }

    public static void Warn(string message)
    {
        Send("[" + Assembly.GetCallingAssembly().GetName().Name + "] " + message, Enums.LogLevel.Warn,
            Enums.LogLevel.Warn.LogLevelTotalColor());
    }

    public static void Error(object message)
    {
        Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", Enums.LogLevel.Error,
            Enums.LogLevel.Error.LogLevelTotalColor());
    }

    public static void Error(string message)
    {
        Send("[" + Assembly.GetCallingAssembly().GetName().Name + "] " + message, Enums.LogLevel.Error,
            Enums.LogLevel.Error.LogLevelTotalColor());
    }

    public static void Debug(object message)
    {
        Assembly callingAssembly = Assembly.GetCallingAssembly();
        if (callingAssembly.GetName().Name == "HeyBoxChatCs.Api")
        {
            Send($"[{callingAssembly.GetName().Name}] {message}", Enums.LogLevel.Debug,
                Enums.LogLevel.Debug.LogLevelTotalColor());
        }
    }

    public static void Debug(string message)
    {
        Assembly callingAssembly = Assembly.GetCallingAssembly();
        if (callingAssembly.GetName().Name == "HeyBoxChatCs.Api")
        {
            Send("[" + callingAssembly.GetName().Name + "] " + message, Enums.LogLevel.Debug,
                Enums.LogLevel.Debug.LogLevelTotalColor());
        }
    }

    public static T DebugObject<T>(T message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Debug(message);
        return message;
    }

    private static void Send(object message, Enums.LogLevel level, ConsoleColor consoleColor)
    {
        SeadRaw($"[{level.ToString().ToUpper()}] {message}", consoleColor);
    }

    public static void Send(string message, Enums.LogLevel level, ConsoleColor consoleColor)
    {
        SeadRaw("[" + level.ToString().ToUpper() + "] " + message, consoleColor);
    }

    public static void SeadRaw(string message, ConsoleColor consoleColor)
    {
        lock (_consoleLock)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine("[" + Misc.GetNowTimeString() + "] " + message);
            Console.ResetColor();
        }
    }

    public static void Assert(bool condition, object message)
    {
        if (condition)
        {
            return;
        }

        Error(message);
        throw new Exception(message.ToString());
    }
}