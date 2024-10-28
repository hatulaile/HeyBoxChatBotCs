using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features;

public class Emoji
{
    [JsonPropertyName("user_info")] public required User User { get; init; }
    [JsonPropertyName("meme_info")] public required MemeInfo MemeInfo { get; init; }
}