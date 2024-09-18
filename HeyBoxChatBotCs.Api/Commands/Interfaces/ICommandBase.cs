namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface ICommandBase
{
    public string Command { get; }
    public string[]? Aliases { get; }
    public string Description { get; }
}