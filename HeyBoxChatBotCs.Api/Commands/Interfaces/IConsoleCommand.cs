namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface IConsoleCommand : ICommandBase
{
    public Task<string> Execute(ArraySegment<string> args);
}