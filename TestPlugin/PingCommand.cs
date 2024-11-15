using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.RequestParameters.Message;

namespace TestPlugin;

[CommandHandler(typeof(UserCommandHandler))]
public class PingCommand : IUserCommand
{
    public async Task<string> Execute(UserCommandArgs args)
    {
        await Bot.Instance.SendMessageAsync(new SendMarkdownMessageParams("""
                                                                | Id | Name | Age |
                                                                |:--:|:----:|:---:|
                                                                |  0 | Jack |  12 |
                                                                |  1 | Alex |  17 |
                                                                |  2 | Evan |  22 |
                                                                """,args.Room,args.Channel));
        return string.Empty;
        //  return Task.FromResult($"Hello {args.User.Name}.PONG.");
    }

    public string Command { get; } = "ping";
    public string[]? Aliases { get; } = [];
    public string Description { get; } = "测试";
}