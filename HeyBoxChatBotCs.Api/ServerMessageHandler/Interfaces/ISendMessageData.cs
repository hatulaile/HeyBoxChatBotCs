using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;

public interface ISendMessageData : IData
{
    [JsonPropertyName("msg_id")] string MessageId { get; init; }
    [JsonPropertyName("send_time")] long SendTime { get; init; }
}