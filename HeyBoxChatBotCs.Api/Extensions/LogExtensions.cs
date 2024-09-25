using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Extensions;

public static class LogExtensions
{
    public static ConsoleColor LogLevelTotalColor(this LogLevel level)
    {
        return level switch
        {
            LogLevel.Info => ConsoleColor.Cyan,
            LogLevel.Warn => ConsoleColor.Magenta,
            LogLevel.Error => ConsoleColor.DarkRed,
            LogLevel.Debug => ConsoleColor.Green,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}