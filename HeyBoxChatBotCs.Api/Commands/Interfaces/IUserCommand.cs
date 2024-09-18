using HeyBoxChatBotCs.Api.Commands.CommandSystem;

namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface IUserCommand : ICommandBase
{
    public bool Execute(UserCommandArgs args, ICommandSender? sender, out string response);
}