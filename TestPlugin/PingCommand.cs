using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features;

namespace TestPlugin;

[CommandHandler(typeof(UserCommandHandler))]
public class PingCommand : IUserCommand
{
    public Task<string> Execute(UserCommandArgs args)
    {
        return Task.FromResult($"Hello {args.User.Name ?? "Unknown"}.PONG.");
    }

    public string Command { get; } = "ping";
    public string[]? Aliases { get; } = [];
    public string Description { get; } = "测试";
}