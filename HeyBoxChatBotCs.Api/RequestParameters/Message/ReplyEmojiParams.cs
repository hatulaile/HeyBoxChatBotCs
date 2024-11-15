using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Converters;

namespace HeyBoxChatBotCs.Api.RequestParameters.Message;

public class ReplyEmojiParams
{
    public ReplyEmojiParams(string messageId, string channelId, Features.Emote.Emoji emoji, bool isAdd) : this(
        messageId, channelId, emoji.ToString(), isAdd, emoji.EmoteRoomId)
    {
    }

    public ReplyEmojiParams(string messageId, string channelId, string emoji, bool isAdd, string roomId)
    {
        MessageId = messageId;
        Emoji = emoji;
        IsAdd = isAdd;
        ChannelId = channelId;
        RoomId = roomId;
    }

    [JsonPropertyName("msg_id")] public string MessageId { get; init; }
    [JsonPropertyName("emoji")] public string Emoji { get; init; }

    [JsonPropertyName("is_add"), JsonConverter(typeof(NumberBooleanJsonConverter))]
    public bool IsAdd { get; init; }

    [JsonPropertyName("channel_id")] public string ChannelId { get; init; }
    [JsonPropertyName("room_id")] public string RoomId { get; init; }
}