using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.User;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataHandlers.User;

public class UserSendCommandHandler : IDataHandler
{
    public async Task ProcessDataAsync(object? serverMessage)
    {
        if (serverMessage is not ServerMessage<UserSendCommandData> message)
        {
            return;
        }

        await UserCommandProcessor.InvokeUserSendCommandActionAsync(message.Data);
        await Events.User.OnUserSendCommandAsync(message.Data);
    }
}