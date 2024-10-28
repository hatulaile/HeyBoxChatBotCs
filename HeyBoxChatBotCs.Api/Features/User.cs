using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features;

public class User
{
    [JsonPropertyName("nickname")] public required string Name { get; init; }
    [JsonPropertyName("user_id")] public required long UserId { get; init; }
    [JsonPropertyName("bot")] public required bool IsBot { get; init; }
    [JsonPropertyName("level")] public required int Level { get; init; }
    [JsonPropertyName("roles")] public string[]? Roles { get; init; }
    [JsonPropertyName("room_nickname")] public required string NickName { get; init; }
    [JsonPropertyName("avatar")] public Uri? Avatar { get; init; }

    [JsonPropertyName("avatar_decoration")]
    public required AvatarDecoration Decoration { get; init; }


    public class AvatarDecoration
    {
        [JsonPropertyName("src_type")] public required string Type { get; init; }
        [JsonPropertyName("src_url")] public required Uri Uri { get; init; }
    }
}