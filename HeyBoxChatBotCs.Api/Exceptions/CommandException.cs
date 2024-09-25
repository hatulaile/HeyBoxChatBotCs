using HeyBoxChatBotCs.Api.Commands.Interfaces;

namespace HeyBoxChatBotCs.Api.Exceptions;

public class CommandException : Exception
{
    protected string MessageStr;

    public CommandException(string? message = null, string? command = null)
    {
        MessageStr = string.IsNullOrWhiteSpace(message) ? "命令异常" : message;
        Command = command;
    }

    public CommandException(ICommandBase? command, string? message = null) : this(message, command?.GetType().Name)
    {
    }


    public string? Command { get; }

    public override string Message
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Command))
            {
                MessageStr = $"{Message} {Command}";
            }

            return MessageStr;
        }
    }
}