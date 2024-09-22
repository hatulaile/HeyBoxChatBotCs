using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features;

public class Room
{
    [JsonPropertyName("room_avatar")] public string? Avatar { get; init; }
    [JsonPropertyName("room_id")] public required string Id { get; init; }
    [JsonPropertyName("room_name")] public required string Name { get; init; }
}