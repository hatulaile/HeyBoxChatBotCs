using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.User;

public class UserSendCommandData : IServerMessageData
{
    [JsonPropertyName("bot_id")] public required long BotId { get; init; }

    [JsonPropertyName("channel_base_info")]
    public required Channel Channel { get; init; }


    [JsonPropertyName("command_info")] public required CommandInfo Command { get; init; }

    [JsonPropertyName("room_base_info")] public required Features.Room Room { get; init; }
    [JsonPropertyName("msg_id")] public required string MessageId { get; init; }
    [JsonPropertyName("send_time")] public required long SendTime { get; init; }
    [JsonPropertyName("sender_info")] public required Features.User User { get; init; }

}