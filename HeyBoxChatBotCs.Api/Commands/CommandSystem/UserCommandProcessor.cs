using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.Features.BotRequestDto.Message;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.User;

namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public static class UserCommandProcessor
{
    public const string REPLY_MESSAGE = """@{id:%USERID%} %MESSAGE%""";

    public static readonly UserCommandHandler UserCommandHandler = UserCommandHandler.Create();

    public static event UserSendCommandAction? UserSendCommandAction;

    internal static async Task InvokeUserSendCommandActionAsync(UserSendCommandData commandInfo)
    {
        UserSendCommandAction?.Invoke(commandInfo);
        await ProcessorInputAsync(commandInfo);
    }

    internal static async Task ProcessorInputAsync(UserSendCommandData data)
    {
        try
        {
            string commandStr = data.Command.Message.TrimStart('/', ' ', '\\', '.');
            Log.Debug($"用户输入命令:{commandStr}");
            Log.Debug(Misc.Misc.IsArrayNullOrEmpty(data.Command.Options)
                ? "用户无输入参数"
                : "用户输入参数+" + string.Join(' ',
                    data.Command.Options!.ToDictionary(x => x.Type.ToString(), x => $"Name:{x.Name},Value:{x.Value}")));
            if (!UserCommandHandler.TryGetCommand(commandStr, out ICommandBase? command))
            {
                return;
            }

            if (command is not IUserCommand userCommand)
            {
                return;
            }

            string response = await userCommand.Execute(
                new UserCommandArgs(data.User, data.MessageId, data.SendTime, data.Command, data.Channel, data.Room));
            if (!string.IsNullOrWhiteSpace(response))
            {
                Log.Debug($"返回给用户:{response}");
                await Bot.Instance.SendMessageAsync(
                    new MarkdownMessageDto(
                        REPLY_MESSAGE.Replace("%USERID%", data.User.UserId.ToString()).Replace("%MESSAGE%", response),
                        data.Room, data.Channel, data.MessageId, [data.User]));
            }
            else
            {
                Log.Debug("命令不返回内容或为空!");
            }
        }
        catch (Exception ex)
        {
            Log.Error("处理用户命令发生错误:" + ex);
        }
    }
}