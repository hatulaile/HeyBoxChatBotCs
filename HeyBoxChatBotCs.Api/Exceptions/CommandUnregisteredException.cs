using HeyBoxChatBotCs.Api.Commands.Interfaces;

namespace HeyBoxChatBotCs.Api.Exceptions;

public class CommandUnregisteredException : CommandException
{
    public CommandUnregisteredException(ICommand command, string message = "命令未注册") : this(message,
        command.GetType().Name)
    {
    }

    public CommandUnregisteredException(string? command = null, string? message = "命令未注册") : base(message, command)
    {
    }
}