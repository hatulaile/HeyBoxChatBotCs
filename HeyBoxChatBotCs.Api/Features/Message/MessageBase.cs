using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.Message;

public abstract class MessageBase
{
    protected MessageBase(string ackId, MessageType type, string channelId, string roomId)
    {
        AckId = ackId;
        Type = type;
        ChannelId = channelId;
        RoomId = roomId;
    }

    [JsonPropertyName("heychat_ack_id")] public string AckId { get; init; }
    [JsonPropertyName("msg_type")] public MessageType Type { get; init; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; init; }
    [JsonPropertyName("room_id")] public string RoomId { get; init; }
}