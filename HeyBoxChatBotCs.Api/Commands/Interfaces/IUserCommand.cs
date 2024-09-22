using HeyBoxChatBotCs.Api.Commands.CommandSystem;

namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface IUserCommand : ICommandBase
{
    public Task<string> Execute(UserCommandArgs args);
}