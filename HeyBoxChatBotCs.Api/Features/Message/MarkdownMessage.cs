using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.Message;

public class MarkdownMessage : MessageBase
{
    public MarkdownMessage(string message, Room room, Channel channel, string replyMessageId = "",
        User[]? atUser = null,
        string[]? atRole = null, Channel[]? mentionChannel = null, MessageAddition? addition = null,
        string? ackId = null) : this(
        message,
        room.Id, channel.Id, replyMessageId, atUser?.Select(x => x.UserId.ToString()).ToArray(), atRole,
        mentionChannel?.Select(x => x.Id).ToArray(), addition, ackId)
    {
    }

    public MarkdownMessage(string message, string roomId, string channelId, string replyMessageId = "",
        string[]? atUser = null,
        string[]? atRole = null, string[]? mentionChannelId = null, MessageAddition? addition = null,
        string? ackId = null) : base(
        ackId ?? MessageAck.GetAckId(),
        MessageType.MarkdownPing, channelId, roomId, replyMessageId, addition)
    {
        Message = message;
        AtUserId = string.Join(',', atUser ?? []);
        AtRoleId = string.Join(',', atRole ?? []);
        MentionChannelId = string.Join(',', mentionChannelId ?? []);
    }

    [JsonPropertyName("msg")] public string Message { get; init; }
    [JsonPropertyName("at_user_id")] public string AtUserId { get; init; }
    [JsonPropertyName("at_role_id")] public string AtRoleId { get; init; }

    [JsonPropertyName("mention_channel_id")]
    public string MentionChannelId { get; init; }
}