using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.Features.Bot;
using HeyBoxChatBotCs.Api.Features.Message;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData;

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
        if (Bot.Instance is null)
        {
            return;
        }

        string commandStr = data.Command.Message.TrimStart('/', ' ', '\\', '.');
        Log.Debug($"用户输入命令:{commandStr}");
        Log.Debug(Misc.Misc.IsArrayNullOrEmpty(data.Command.Options)
            ? "用户无输入参数"
            : "用户输入参数+" + string.Join(' ',
                data.Command.Options!.ToDictionary(x => x.Type.ToString(), x => $"Name:{x.Name},Value:{x.Value}")));
        if (!UserCommandHandler.TryGetCommandAsync(commandStr, out ICommandBase? command))
        {
            return;
        }

        if (command is not IUserCommand userCommand)
        {
            return;
        }


        if (userCommand.Execute(
                new UserCommandArgs(data.MessageId, data.SendTime, data.Command, data.Channel, data.Room), data.User,
                out string response) && !string.IsNullOrWhiteSpace(response))
        {
            Log.Debug($"返回给用户:{response}");
            await Bot.Instance.SendMessageAsync(
                new MarkdownMessage(
                    REPLY_MESSAGE.Replace("%USERID%", data.User.UserId.ToString()).Replace("%MESSAGE%", response),
                    data.Room, data.Channel, data.MessageId, [data.User]));
        }
        else
        {
            Log.Debug("命令不返回内容或为空!");
        }
    }
}