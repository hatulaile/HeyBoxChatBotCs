using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features;

public class MemeInfo
{
    [JsonPropertyName("name")] public required string Name { get; init; }
    [JsonPropertyName("path")] public required string Path { get; init; }
    [JsonPropertyName("ext")] public required string Extensions { get; init; }
    [JsonPropertyName("create_time")] public required int CreateTime { get; init; }
    [JsonPropertyName("mtype")] public required EmojiTypeId Type { get; init; }
}