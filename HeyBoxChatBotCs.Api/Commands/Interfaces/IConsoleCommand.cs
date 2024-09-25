namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface IConsoleCommand : ICommandBase
{
    Task<string> Execute(ArraySegment<string> args);
}