using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features;

public class Room
{
    [JsonPropertyName("room_avatar")] public string? Avatar { get; init; }
    [JsonPropertyName("room_id")] public string Id { get; init; }
    [JsonPropertyName("room_name")] public string Name { get; init; }
}