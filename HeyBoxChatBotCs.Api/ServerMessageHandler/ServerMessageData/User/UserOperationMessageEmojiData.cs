using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.User;

public class UserOperationMessageEmojiData : IServerMessageData
{
    [JsonPropertyName("channel_id")] public required string ChannelId { get; init; }

    [JsonPropertyName("emoji")] public required string Emoji { get; init; }

    [JsonPropertyName("is_add")] public required ReactionActionType ReactionActionType { get; init; }

    [JsonPropertyName("msg_id")] public required string MessageId { get; init; }

    [JsonPropertyName("user_id")] public required int UserId { get; init; }
}