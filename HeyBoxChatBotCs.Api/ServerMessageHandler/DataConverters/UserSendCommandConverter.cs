using System.Text.Json;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataConverters;

public class UserSendCommandConverter : IDataConverter
{
    public Task<object?> ConverterAsync(string message)
    {
        return Task.FromResult<object?>(JsonSerializer.Deserialize<ServerMessage<UserSendCommandData>>(message));
    }
}