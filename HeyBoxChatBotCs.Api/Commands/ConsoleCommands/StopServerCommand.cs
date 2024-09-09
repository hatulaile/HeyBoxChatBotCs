using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features.Bot;
using static System.Int32;

namespace HeyBoxChatBotCs.Api.Commands.ConsoleCommands;

public class StopServerCommand : ICommand
{
    public string Command { get; } = "stop";
    public string[]? Aliases { get; } = [];
    public string Description { get; } = "停止全部程序运行";

    public bool Execute(ArraySegment<string> args, ICommandSender? sender, out string response)
    {
        Bot.Instance?.Close();
        int code = 0;
        if (args.Count != 0)
        {
            if (!TryParse(args.ElementAt(0), out code))
            {
                code = 0;
            }
        }

        Misc.Exit(code);
        //下面不会执行
        response = string.Empty;
        return true;
    }
}