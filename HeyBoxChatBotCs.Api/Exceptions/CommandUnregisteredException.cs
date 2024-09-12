using HeyBoxChatBotCs.Api.Commands.Interfaces;

namespace HeyBoxChatBotCs.Api.Exceptions;

public class CommandUnregisteredException : CommandException
{
    public CommandUnregisteredException(ICommandBase commandBase, string message = "命令未注册") : this(message,
        commandBase.GetType().Name)
    {
    }

    public CommandUnregisteredException(string? command = null, string? message = "命令未注册") : base(message, command)
    {
    }
}