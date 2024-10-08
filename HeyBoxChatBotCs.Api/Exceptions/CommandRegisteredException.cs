﻿using HeyBoxChatBotCs.Api.Commands.Interfaces;

namespace HeyBoxChatBotCs.Api.Exceptions;

public class CommandRegisteredException : CommandException
{
    public CommandRegisteredException(ICommandBase commandBase, string message = "命令已注册") : this(message,
        commandBase.GetType().Name)
    {
    }

    public CommandRegisteredException(string? command = null, string? message = "命令已注册") : base(message, command)
    {
    }
}