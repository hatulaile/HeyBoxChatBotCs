using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestResponse;

internal class RoomEmojiResponse
{
    [JsonPropertyName("emoji")] public required RoomEmoji[] Emojis { get; init; }
    [JsonPropertyName("sticker")] public required RoomEmoji[] Stickers { get; init; }

    internal class RoomEmoji
    {
        [JsonPropertyName("user_info")] public required User User { get; init; }
        [JsonPropertyName("meme_info")] public required MemeInfo MemeInfo { get; init; }
    }
}