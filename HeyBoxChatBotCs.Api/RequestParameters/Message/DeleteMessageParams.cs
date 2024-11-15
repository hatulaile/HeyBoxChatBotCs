using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.RequestParameters.Message;

public class DeleteMessageParams
{
    public DeleteMessageParams(string messageId, string type, string channelId)
    {
        MessageId = messageId;
        Type = type;
        ChannelId = channelId;
    }

    [JsonPropertyName("msg_id")] public string MessageId { get; set; }
    [JsonPropertyName("room_id")] public string Type { get; set; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; set; }
}