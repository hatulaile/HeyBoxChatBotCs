using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Commands.Interfaces;

namespace HeyBoxChatBotCs.Api.Features;

public class User : ICommandSender
{
    [JsonPropertyName("nickname")] public required string Name { get; init; }
    [JsonPropertyName("user_id")] public long UserId { get; init; }
    [JsonPropertyName("bot")] public bool IsBot { get; init; }
    [JsonPropertyName("level")] public int Level { get; init; }
    [JsonPropertyName("roles")] public string[]? Roles { get; init; }
    [JsonPropertyName("room_nickname")] public required string NickName { get; init; }
    [JsonPropertyName("avatar")] public Uri? Avatar { get; init; }

    [JsonPropertyName("avatar_decoration")]
    public required AvatarDecoration Decoration { get; init; }

    public static User? Get(ICommandSender? sender)
    {
        return sender as User;
    }

    public class AvatarDecoration
    {
        [JsonPropertyName("src_type")] public required string Type { get; init; }
        [JsonPropertyName("src_url")] public required Uri Uri { get; init; }
    }
}