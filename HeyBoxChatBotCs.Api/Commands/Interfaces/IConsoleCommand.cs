namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface IConsoleCommand : ICommandBase
{
    public bool Execute(ArraySegment<string> args, out string response);
}