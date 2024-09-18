using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.Message;

[JsonDerivedType(typeof(MarkdownMessage))]
[JsonDerivedType(typeof(ImageMessage))]
public abstract class MessageBase
{
    protected MessageBase(string ackId, MessageType type, string channelId, string roomId, string replyMessageId = "",
        MessageAddition? addition = null)
    {
        ReplyMessageId = replyMessageId;
        AckId = ackId;
        Type = type;
        ChannelId = channelId;
        RoomId = roomId;
        Addition = JsonSerializer.Serialize(addition ?? new MessageAddition(), Bot.Bot.BotActionJsonSerializerOptions);
    }


    [JsonPropertyName("reply_id")] public string ReplyMessageId { get; init; }
    [JsonPropertyName("heychat_ack_id")] public string AckId { get; init; }
    [JsonPropertyName("msg_type")] public MessageType Type { get; init; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; init; }
    [JsonPropertyName("room_id")] public string RoomId { get; init; }

    [JsonPropertyName("addition")] public string Addition { get; init; }
}

public class MessageAddition
{
    public MessageAddition(MessageAdditionImage[]? additionImages = null)
    {
        ImageFilesInfo = additionImages ?? [];
    }

    [JsonPropertyName("img_files_info")] public MessageAdditionImage[] ImageFilesInfo { get; init; }
}

public class MessageAdditionImage
{
    public MessageAdditionImage(string uri, int width, int height) : this(new Uri(uri), width,
        height)
    {
    }

    public MessageAdditionImage(Uri uri, int width, int height)
    {
        Uri = uri;
        Width = width;
        Height = height;
    }

    [JsonPropertyName("uri")] public Uri Uri { get; init; }
    [JsonPropertyName("width")] public int Width { get; init; }
    [JsonPropertyName("height")] public int Height { get; init; }
}