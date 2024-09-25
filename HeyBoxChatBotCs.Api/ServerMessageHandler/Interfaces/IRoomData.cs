using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;

public interface IRoomData : IData
{
    [JsonPropertyName("room_base_info")] Room Room { get; init; }
}