using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestParameters.Message;

public class SendImageMessageParams : SendMessageParamsBase
{
    public SendImageMessageParams(MessageAdditionImage image, Channel channelId, Room roomId, string replyMessageId = "",
        string? ackId = null) : this(image, channelId.Id, roomId.Id, replyMessageId, ackId)
    {
    }

    public SendImageMessageParams(MessageAdditionImage image, string channelId, string roomId, string replyMessageId = "",
        string? ackId = null) : base(ackId ?? RequestAck.GetAckId(), MessageType.Image, channelId, roomId,
        replyMessageId, new MessageAddition([image]))
    {
        Uri = image.Uri;
    }


    public SendImageMessageParams(string image, string channelId, string roomId, string replyMessageId = "", string? ackId = null)
        : this(new Uri(image),
            channelId, roomId, replyMessageId, ackId)
    {
    }

    public SendImageMessageParams(Uri image, Channel channelId, Room roomId, string replyMessageId = "", string? ackId = null) :
        this(image,
            channelId.Id, roomId.Id, replyMessageId, ackId)
    {
    }

    public SendImageMessageParams(Uri image, string channelId, string roomId, string replyMessageId = "",
        string? ackId = null) : base(
        ackId ?? RequestAck.GetAckId(), MessageType.Image, channelId, roomId, replyMessageId)
    {
        Uri = image;
    }

    [JsonPropertyName("img")] public Uri Uri { get; init; }
}