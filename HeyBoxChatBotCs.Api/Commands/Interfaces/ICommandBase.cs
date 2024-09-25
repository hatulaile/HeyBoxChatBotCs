namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface ICommandBase
{
    string Command { get; }
    string[]? Aliases { get; }
    string Description { get; }
}