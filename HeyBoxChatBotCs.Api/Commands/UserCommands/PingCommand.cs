﻿using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.Commands.UserCommands;

public class PingCommand : IUserCommand
{
    public bool Execute(CommandInfo.CommandOption[] args, ICommandSender? sender, out string response)
    {
        response = $"Hello {User.Get(sender)?.Name ?? "Unknown"}.PONG";
        return true;
    }

    public string Command { get; } = "ping";
    public string[]? Aliases { get; } = [];
    public string Description { get; } = "测试";
}