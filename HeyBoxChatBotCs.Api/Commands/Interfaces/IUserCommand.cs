using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface IUserCommand : ICommandBase
{
    public bool Execute(UserCommandArgs args, ICommandSender? sender, out string response);
}