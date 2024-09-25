using HeyBoxChatBotCs.Api.Commands.CommandSystem;

namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface IUserCommand : ICommandBase
{
    Task<string> Execute(UserCommandArgs args);
}