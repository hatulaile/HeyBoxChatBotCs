using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestParameters.Message;

[JsonDerivedType(typeof(SendMarkdownMessageParams))]
[JsonDerivedType(typeof(SendImageMessageParams))]
public abstract class SendMessageParamsBase
{
    protected SendMessageParamsBase(string ackId, MessageType type, string channelId, string roomId,
        string replyMessageId = "",
        MessageAddition? addition = null)
    {
        ReplyMessageId = replyMessageId;
        AckId = ackId;
        Type = type;
        ChannelId = channelId;
        RoomId = roomId;
        Addition = addition ?? new MessageAddition();
    }


    [JsonPropertyName("reply_id")] public string ReplyMessageId { get; protected set; }
    [JsonPropertyName("heychat_ack_id")] public string AckId { get; protected set; }
    [JsonPropertyName("msg_type")] public MessageType Type { get; protected set; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; protected set; }
    [JsonPropertyName("room_id")] public string RoomId { get; protected set; }

    [JsonPropertyName("addition")]
    public string AdditionStr => JsonSerializer.Serialize(Addition, Bot.BotOperationJsonSerializerOptions);

    [JsonIgnore] public MessageAddition Addition { get; protected set; }
}

public class MessageAddition
{
    public MessageAddition(MessageAdditionImage[]? additionImages = null)
    {
        ImageFilesInfo = additionImages ?? [];
    }

    [JsonPropertyName("img_files_info")] public MessageAdditionImage[] ImageFilesInfo { get; set; }
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

    [JsonPropertyName("uri")] public Uri Uri { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}