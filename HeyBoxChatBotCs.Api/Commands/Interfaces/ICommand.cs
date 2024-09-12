namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface ICommand<in TArgs> : ICommandBase
{
    public bool Execute(TArgs args, ICommandSender? sender, out string response);
}