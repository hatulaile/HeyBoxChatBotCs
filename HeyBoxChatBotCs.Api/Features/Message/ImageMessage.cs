using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.Message;

public class ImageMessage : MessageBase
{
    public ImageMessage(MessageAdditionImage image, Channel channelId, Room roomId, string replyMessageId = "",
        string? ackId = null) : this(image, channelId.Id, roomId.Id, replyMessageId, ackId)
    {
    }

    public ImageMessage(MessageAdditionImage image, string channelId, string roomId, string replyMessageId = "",
        string? ackId = null) : base(ackId ?? MessageAck.GetAckId(), MessageType.Image, channelId, roomId,
        replyMessageId, new MessageAddition([image]))
    {
        Uri = image.Uri;
    }


    public ImageMessage(string image, string channelId, string roomId, string replyMessageId = "", string? ackId = null)
        : this(new Uri(image),
            channelId, roomId, replyMessageId, ackId)
    {
    }

    public ImageMessage(Uri image, Channel channelId, Room roomId, string replyMessageId = "", string? ackId = null) :
        this(image,
            channelId.Id, roomId.Id, replyMessageId, ackId)
    {
    }

    public ImageMessage(Uri image, string channelId, string roomId, string replyMessageId = "",
        string? ackId = null) : base(
        ackId ?? MessageAck.GetAckId(), MessageType.Image, channelId, roomId, replyMessageId)
    {
        Uri = image;
    }

    [JsonPropertyName("img")] public Uri Uri { get; init; }
}