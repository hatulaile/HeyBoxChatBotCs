using System.Reflection;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Extensions;

namespace HeyBoxChatBotCs.Api.Features;

public static class Log
{
    private static readonly object ConsoleLock = new();
    public static HashSet<Assembly> DebugEnabled { get; } = [];

    public static void Info(object message)
    {
        Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Info,
            LogLevel.Info.LogLevelTotalColor());
    }

    public static void Info(string message)
    {
        Send("[" + Assembly.GetCallingAssembly().GetName().Name + "] " + message, LogLevel.Info,
            LogLevel.Info.LogLevelTotalColor());
    }

    public static void Warn(object message)
    {
        Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Warn,
            LogLevel.Warn.LogLevelTotalColor());
    }

    public static void Warn(string message)
    {
        Send("[" + Assembly.GetCallingAssembly().GetName().Name + "] " + message, LogLevel.Warn,
            LogLevel.Warn.LogLevelTotalColor());
    }

    public static void Error(object message)
    {
        Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {message}", LogLevel.Error,
            LogLevel.Error.LogLevelTotalColor());
    }

    public static void Error(string message)
    {
        Send("[" + Assembly.GetCallingAssembly().GetName().Name + "] " + message, LogLevel.Error,
            LogLevel.Error.LogLevelTotalColor());
    }

    public static void Debug(object message)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
#if DEBUG
        if (callingAssembly.GetName().Name == "HeyBoxChatBotCs.Api")
            Send("[" + callingAssembly.GetName().Name + "] " + message, LogLevel.Debug,
                LogLevel.Debug.LogLevelTotalColor());
#endif

        if (DebugEnabled.Contains(callingAssembly))
            Send("[" + callingAssembly.GetName().Name + "] " + message, LogLevel.Debug,
                LogLevel.Debug.LogLevelTotalColor());
    }

    public static void Debug(string message)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
#if DEBUG
        if (callingAssembly.GetName().Name == "HeyBoxChatBotCs.Api")
            Send("[" + callingAssembly.GetName().Name + "] " + message, LogLevel.Debug,
                LogLevel.Debug.LogLevelTotalColor());
#endif

        if (DebugEnabled.Contains(callingAssembly))
            Send("[" + callingAssembly.GetName().Name + "] " + message, LogLevel.Debug,
                LogLevel.Debug.LogLevelTotalColor());
    }

    public static T DebugObject<T>(T message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Debug(message);
        return message;
    }

    private static void Send(object message, LogLevel level, ConsoleColor consoleColor)
    {
        SeadRaw($"[{level.ToString().ToUpper()}] {message}", consoleColor);
    }

    public static void Send(string message, LogLevel level, ConsoleColor consoleColor)
    {
        SeadRaw("[" + level.ToString().ToUpper() + "] " + message, consoleColor);
    }

    public static void SeadRaw(string message, ConsoleColor consoleColor)
    {
        lock (ConsoleLock)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine("[" + Misc.Misc.GetNowTimeString() + "] " + message);
            Console.ResetColor();
        }
    }

    public static void Assert(bool condition, object message)
    {
        if (condition) return;

        Error(message);
        throw new Exception(message.ToString());
    }
}