using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotRequestDto;

public class DeleteMessageDto
{
    public DeleteMessageDto(string messageId, string type, string channelId)
    {
        MessageId = messageId;
        Type = type;
        ChannelId = channelId;
    }

    [JsonPropertyName("msg_id")] public string MessageId { get; set; }
    [JsonPropertyName("room_id")] public string Type { get; set; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; set; }
}