using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.System;

public class ServerMessage<TData> : IServerMessage<TData> where TData : IServerMessageData
{
    [JsonPropertyName("sequence")] public required long Sequence { get; init; }
    [JsonPropertyName("type")] public required string Type { get; init; }
    [JsonPropertyName("notify_type")] public required string NotifyType { get; init; }
    [JsonPropertyName("data")] public required TData Data { get; init; }
    [JsonPropertyName("timestamp")] public required long Timestamp { get; init; }
}