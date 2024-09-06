﻿using System.Text;
using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api;

namespace HeyBoxChatBotCs.Api.Commands.ConsoleCommands;

public class ConsoleHelpCommand : ICommand
{
    public string Command { get; } = "help";
    public string[]? Aliases { get; } = [];
    public string Description { get; } = "输出全部控制台命令";

    public bool Execute(ArraySegment<string> args, ICommandSender? sender, out string response)
    {
        StringBuilder sb = new StringBuilder("以下为全控制台命令:\n");
        int count = 0;
        foreach (ICommand command in
                 ConsoleCommandProcessor.ConsoleCommandHandler.AllCommand)
        {
            sb.AppendLine(
                $"  {++count}.{command.Command} - {command.Description} {(!Misc.IsArrayNullOrEmpty(command.Aliases) ? $"别名: {string.Join(',', command.Aliases)}" : string.Empty)}");
        }

        sb.Append($"共 {count} 条指令可使用");
        response = sb.ToString();
        return true;
    }
}