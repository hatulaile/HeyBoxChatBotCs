namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface ICommand
{
    public string Command { get; }
    public string[]? Aliases { get; }
    public string Description { get; }

    public bool Execute(ArraySegment<string> args, ICommandSender? sender, out string response);
}