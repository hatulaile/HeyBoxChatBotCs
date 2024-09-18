using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.Bot.BotActionResult;

public class SendMessageResult
{
    public class MessageResult
    {
        [JsonPropertyName("chatmobile_ack_id")]
        public string ChatMobileAckId { get; init; }

        [JsonPropertyName("heychat_ack_id")] public string HeyChatAckId { get; init; }
    }
}