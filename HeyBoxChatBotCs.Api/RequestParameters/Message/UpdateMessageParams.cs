using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestParameters.Message;

public class UpdateMessageParams
{
    public UpdateMessageParams(string messageId, SendMarkdownMessageParams sendMessageParams)
    {
        MessageId = messageId;
        Message = sendMessageParams.Message;
        AtUserId = sendMessageParams.AtUserId;
        AtRoleId = sendMessageParams.AtRoleId;
        MentionChannelId = sendMessageParams.MentionChannelId;
        ReplyMessageId = sendMessageParams.ReplyMessageId;
        AckId = RequestAck.GetAckId();
        Type = sendMessageParams.Type;
        ChannelId = sendMessageParams.ChannelId;
        RoomId = sendMessageParams.RoomId;
        Addition = sendMessageParams.Addition;
    }

    [JsonPropertyName("msg_id")] public string MessageId { get; set; }
    [JsonPropertyName("msg")] public string Message { get; set; }
    [JsonPropertyName("at_user_id")] public string AtUserId { get; set; }
    [JsonPropertyName("at_role_id")] public string AtRoleId { get; set; }

    [JsonPropertyName("mention_channel_id")]
    public string MentionChannelId { get; set; }

    [JsonPropertyName("reply_id")] public string ReplyMessageId { get; protected set; }
    [JsonPropertyName("heychat_ack_id")] public string AckId { get; protected set; }
    [JsonPropertyName("msg_type")] public MessageType Type { get; protected set; }
    [JsonPropertyName("channel_id")] public string ChannelId { get; protected set; }
    [JsonPropertyName("room_id")] public string RoomId { get; protected set; }

    [JsonPropertyName("addition")]
    public string AdditionStr => JsonSerializer.Serialize(Addition, Bot.BotOperationJsonSerializerOptions);

    [JsonIgnore] public MessageAddition Addition { get; protected set; }
}