using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.EventArgs.Interfaces;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.Features.Bot;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageDatas;

namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public static class UserCommandProcessor
{
    public static readonly UserCommandHandler UserCommandHandler = CommandSystem.UserCommandHandler.Create();

    public static event UserSendCommandAction UserSendCommandAction = ProcessorInput;

    internal static void InvokeUserSendCommandAction(UserSendCommandData commandInfo)
    {
        UserSendCommandAction?.Invoke(commandInfo);
    }

    internal static void ProcessorInput(UserSendCommandData data)
    {
        if (Bot.Instance is null)
        {
            return;
        }

        string commandStr = data.Command.Message.TrimStart('/', ' ', '\\', '.');
        Log.Debug($"用户输入命令:{commandStr}");
        if (!UserCommandHandler.TryGetCommand(commandStr, out ICommandBase? command))
        {
            return;
        }

        if (command is not IUserCommand userCommand)
        {
            return;
        }

        userCommand.Execute(data.Command.Options, data.User, out string response);
        Log.Debug($"返回给用户:{response}");
        Bot.Instance.SendMessage(response);
    }
}