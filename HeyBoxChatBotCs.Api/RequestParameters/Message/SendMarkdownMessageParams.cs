using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestParameters.Message;

public class SendMarkdownMessageParams : SendMessageParamsBase
{
    public SendMarkdownMessageParams(string message, Room room, Channel channel, string replyMessageId = "",
        User[]? atUser = null,
        string[]? atRole = null, Channel[]? mentionChannel = null, MessageAddition? addition = null,
        string? ackId = null) : this(
        message,
        room.Id, channel.Id, replyMessageId, atUser?.Select(x => x.UserId.ToString()).ToArray(), atRole,
        mentionChannel?.Select(x => x.Id).ToArray(), addition, ackId)
    {
    }

    public SendMarkdownMessageParams(string message, string roomId, string channelId, string replyMessageId = "",
        string[]? atUser = null,
        string[]? atRole = null, string[]? mentionChannelId = null, MessageAddition? addition = null,
        string? ackId = null) : base(
        ackId ?? RequestAck.GetAckId(),
        MessageType.MarkdownPing, channelId, roomId, replyMessageId, addition)
    {
        Message = message;
        AtUserId = string.Join(',', atUser ?? []);
        AtRoleId = string.Join(',', atRole ?? []);
        MentionChannelId = string.Join(',', mentionChannelId ?? []);
    }

    public SendMarkdownMessageParams(string message, string roomId, string channelId, string replyMessageId,
        string atUser,
        string atRole, string mentionChannelId, MessageAddition addition,
        string? ackId = null) : base(
        ackId ?? RequestAck.GetAckId(),
        MessageType.MarkdownPing, channelId, roomId, replyMessageId, addition)
    {
        Message = message;
        AtUserId = atUser;
        AtRoleId = atRole;
        MentionChannelId = mentionChannelId;
    }

    [JsonPropertyName("msg")] public string Message { get; set; }
    [JsonPropertyName("at_user_id")] public string AtUserId { get; set; }
    [JsonPropertyName("at_role_id")] public string AtRoleId { get; set; }

    [JsonPropertyName("mention_channel_id")]
    public string MentionChannelId { get; set; }
}