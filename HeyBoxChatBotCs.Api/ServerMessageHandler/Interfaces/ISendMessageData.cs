using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;

public interface ISendMessageData : IData
{
    [JsonPropertyName("msg_id")] public string MessageId { get; init; }
    [JsonPropertyName("send_time")] public long SendTime { get; init; }
}