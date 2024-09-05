namespace HeyBoxChatBotCs.Api.Extensions;

public static class LogExtensions
{
    public static ConsoleColor LogLevelTotalColor(this Enums.LogLevel level)
    {
        return level switch
        {
            Enums.LogLevel.Info => ConsoleColor.Cyan,
            Enums.LogLevel.Warn => ConsoleColor.Magenta,
            Enums.LogLevel.Error => ConsoleColor.DarkRed,
            Enums.LogLevel.Debug => ConsoleColor.Green,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
        };
    }
}