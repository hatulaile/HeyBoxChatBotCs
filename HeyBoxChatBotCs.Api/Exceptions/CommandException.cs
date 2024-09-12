using HeyBoxChatBotCs.Api.Commands.Interfaces;

namespace HeyBoxChatBotCs.Api.Exceptions;

public class CommandException : Exception
{
    public CommandException(string? message = null, string? command = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            message = "命令异常";
        }
        else
        {
            MessageStr = message;
        }

        Command = command;
    }

    public CommandException(ICommandBase? command, string? message = null) : this(message, command?.GetType().Name)
    {
    }


    public string? Command { get; }

    protected string MessageStr;

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