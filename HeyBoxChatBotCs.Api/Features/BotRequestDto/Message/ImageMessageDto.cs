using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.BotRequestDto.Message;

public class ImageMessageDto : MessageBase
{
    public ImageMessageDto(MessageAdditionImage image, Channel channelId, Room roomId, string replyMessageId = "",
        string? ackId = null) : this(image, channelId.Id, roomId.Id, replyMessageId, ackId)
    {
    }

    public ImageMessageDto(MessageAdditionImage image, string channelId, string roomId, string replyMessageId = "",
        string? ackId = null) : base(ackId ?? RequestAck.GetAckId(), MessageType.Image, channelId, roomId,
        replyMessageId, new MessageAddition([image]))
    {
        Uri = image.Uri;
    }


    public ImageMessageDto(string image, string channelId, string roomId, string replyMessageId = "", string? ackId = null)
        : this(new Uri(image),
            channelId, roomId, replyMessageId, ackId)
    {
    }

    public ImageMessageDto(Uri image, Channel channelId, Room roomId, string replyMessageId = "", string? ackId = null) :
        this(image,
            channelId.Id, roomId.Id, replyMessageId, ackId)
    {
    }

    public ImageMessageDto(Uri image, string channelId, string roomId, string replyMessageId = "",
        string? ackId = null) : base(
        ackId ?? RequestAck.GetAckId(), MessageType.Image, channelId, roomId, replyMessageId)
    {
        Uri = image;
    }

    [JsonPropertyName("img")] public Uri Uri { get; init; }
}