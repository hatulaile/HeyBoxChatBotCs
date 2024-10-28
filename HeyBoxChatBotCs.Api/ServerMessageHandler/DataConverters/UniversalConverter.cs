using System.Text.Json;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataConverters;

public class UniversalConverter<T> : IDataConverter where T : IServerMessageData
{
    public Task<object?> ConverterAsync(string message)
    {
        return Task.FromResult<object?>(JsonSerializer.Deserialize<ServerMessage<T>>(message));
    }
}