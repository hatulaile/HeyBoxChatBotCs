using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotActionResult;

public class UpdateMarkdownMessageResult
{
    [JsonPropertyName("heychat_ack_id")] public required string HeyChatAckId { get; init; }

    [JsonPropertyName("chatmobile_ack_id")]
    public required string ChatmobileAckId { get; init; }
}