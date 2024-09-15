using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.System;

public class ServerMessage<TData> : IServerMessage<TData> where TData : IServerMessageData, new()
{
    [JsonPropertyName("sequence")] public long Sequence { get; init; }
    [JsonPropertyName("type")] public string Type { get; init; }
    [JsonPropertyName("notify_type")] public string NotifyType { get; init; }
    [JsonPropertyName("data")] public TData Data { get; init; }
    [JsonPropertyName("timestamp")] public long Timestamp { get; init; }
}