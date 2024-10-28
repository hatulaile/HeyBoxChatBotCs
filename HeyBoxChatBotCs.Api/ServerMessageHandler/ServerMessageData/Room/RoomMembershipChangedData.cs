using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.Room;

public class RoomMembershipChangedData : IServerMessageData
{
    [JsonPropertyName("room_base_info")] public required Features.Room Room { get; init; }
    [JsonPropertyName("user_info")] public required Features.User User { get; init; }
    [JsonPropertyName("state")] public required RoomMembershipChangedTypeId State { get; init; }
}