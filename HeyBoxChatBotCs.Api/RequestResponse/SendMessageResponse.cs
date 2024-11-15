using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.RequestResponse;

internal class SendMessageResponse
{
    [JsonPropertyName("chatmobile_ack_id")]
    public required string ChatMobileAckId { get; init; }

    [JsonPropertyName("heychat_ack_id")] public required string HeyChatAckId { get; init; }

    [JsonPropertyName("msg_id")] public required string MessageId { get; init; }
    [JsonPropertyName("msg_seq")] public required string MessageSqe { get; init; }

    public override string ToString()
    {
        return
            $"ChatMobileAckId:{ChatMobileAckId},HeyChatAckId:{HeyChatAckId},MessageId:{MessageId},MessageSqe:{MessageSqe}";
    }
}