using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.Message;

public class MarkdownMessage : MessageBase
{
    public MarkdownMessage(string message, Room room, Channel channel, string replyMessageId = "",
        User[]? atUser = null,
        string[]? atRole = null, Channel[]? mentionChannel = null, MarkdownMessageAddition? addition = null,
        string? ackId = null) : this(
        message,
        room.Id, channel.Id, replyMessageId, atUser?.Select(x => x.UserId.ToString()).ToArray(), atRole,
        mentionChannel?.Select(x => x.Id).ToArray(), addition, ackId)
    {
    }

    public MarkdownMessage(string message, string roomId, string channelId, string replyMessageId = "",
        string[]? atUser = null,
        string[]? atRole = null, string[]? mentionChannelId = null, MarkdownMessageAddition? addition = null,
        string? ackId = null) : base(
        ackId ?? MessageAck.GetAckId(),
        MessageType.MarkdownPing, channelId, roomId)
    {
        Message = message;
        ReplyMessageId = replyMessageId;
        AtUserId = string.Join(',', atUser ?? []);
        AtRoleId = string.Join(',', atRole ?? []);
        MentionChannelId = string.Join(',', mentionChannelId ?? []);
        Addition = JsonSerializer.Serialize(addition ?? new MarkdownMessageAddition());
    }

    [JsonPropertyName("msg")] public string Message { get; init; }
    [JsonPropertyName("reply_id")] public string ReplyMessageId { get; init; }
    [JsonPropertyName("at_user_id")] public string AtUserId { get; init; }
    [JsonPropertyName("at_role_id")] public string AtRoleId { get; init; }

    [JsonPropertyName("mention_channel_id")]
    public string MentionChannelId { get; init; }

    [JsonPropertyName("addition")] public string Addition { get; init; }
}

public class MarkdownMessageAddition
{
    public MarkdownMessageAddition(MarkdownMessageAdditionImage[]? additionImages = null)
    {
        ImageFilesInfo = additionImages ?? [];
    }

    [JsonPropertyName("img_files_info")] public MarkdownMessageAdditionImage[] ImageFilesInfo { get; init; }

    public class MarkdownMessageAdditionImage
    {
        public MarkdownMessageAdditionImage(string uri, int width, int height) : this(new Uri(uri), width, height)
        {
        }

        public MarkdownMessageAdditionImage(Uri uri, int width, int height)
        {
            Uri = uri;
            Width = width;
            Height = height;
        }

        [JsonPropertyName("uri")] public Uri Uri { get; init; }
        [JsonPropertyName("width")] public int Width { get; init; }
        [JsonPropertyName("height")] public int Height { get; init; }
    }
}