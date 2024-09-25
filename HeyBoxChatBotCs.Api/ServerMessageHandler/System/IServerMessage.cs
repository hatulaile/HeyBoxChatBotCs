using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.System;

public interface IServerMessage<TData> where TData : IServerMessageData
{
    [JsonPropertyName("sequence")] long Sequence { get; init; }
    [JsonPropertyName("type")] string Type { get; init; }
    [JsonPropertyName("notify_type")] string NotifyType { get; init; }
    [JsonPropertyName("data")] TData Data { get; init; }
    [JsonPropertyName("timestamp")] long Timestamp { get; init; }
}