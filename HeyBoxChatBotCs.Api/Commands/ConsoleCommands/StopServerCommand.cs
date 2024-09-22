using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features.Bot;

namespace HeyBoxChatBotCs.Api.Commands.ConsoleCommands;

public class StopServerCommand : IConsoleCommand
{
    public string Command { get; } = "stop";
    public string[]? Aliases { get; } = [];
    public string Description { get; } = "停止全部程序运行";

    public Task<string> Execute(ArraySegment<string> args)
    {
#if DEBUG
        int code = 0;
        if (args.Count != 0 && int.TryParse(args.ElementAt(0), out code))
        {
        }

        Misc.Misc.Exit(code);
        return Task.FromResult(string.Empty);
#else
        Bot.Instance?.CloseAsync();
        response = "正在停止程序运行!";
        return true;
#endif
    }
}