using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Commands.Interfaces;

namespace TestPlugin;

[CommandHandler(typeof(UserCommandHandler))]
public class PingCommand : IUserCommand
{
    public Task<string> Execute(UserCommandArgs args)
    {
        return Task.FromResult($"Hello {args.User.Name}.PONG.");
    }

    public string Command { get; } = "ping";
    public string[]? Aliases { get; } = [];
    public string Description { get; } = "测试";
}