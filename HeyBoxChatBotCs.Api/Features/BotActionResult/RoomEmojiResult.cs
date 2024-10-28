using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotActionResult;

public class RoomEmojiResult
{
    [JsonPropertyName("emoji")] public required Emoji[] Emojis { get; init; }
    [JsonPropertyName("sticker")] public required Emoji[] Stickers { get; init; }
}