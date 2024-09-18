using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features.Bot;

namespace HeyBoxChatBotCs.Api.Commands.ConsoleCommands;

public class StopServerCommand : IConsoleCommand
{
    public string Command { get; } = "stop";
    public string[]? Aliases { get; } = [];
    public string Description { get; } = "停止全部程序运行";

    public bool Execute(ArraySegment<string> args, out string response)
    {
        int code = 0;
#if DEBUG
        if (args.Count != 0 && int.TryParse(args.ElementAt(0), out code))
        {
        }

        Misc.Misc.Exit(code);
        response = string.Empty;
        return true;
#endif
        Bot.Instance.Close();
        response = "正在停止程序运行!";
        return true;
    }
}