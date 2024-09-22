using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Features.Events;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataHandlers;

public class UserSendCommandHandler : IDataHandler
{
    public async Task ProcessDataAsync(object? serverMessage)
    {
        if (serverMessage is not ServerMessage<UserSendCommandData> message)
        {
            return;
        }

        await UserCommandProcessor.InvokeUserSendCommandActionAsync(message.Data);
        await User.OnUserSendCommandAsync(message.Data);
    }
}