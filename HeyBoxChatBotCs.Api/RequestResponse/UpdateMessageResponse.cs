using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.RequestResponse;

internal class UpdateMessageResponse
{
    [JsonPropertyName("heychat_ack_id")] public required string HeyChatAckId { get; init; }

    [JsonPropertyName("chatmobile_ack_id")]
    public required string ChatmobileAckId { get; init; }
}