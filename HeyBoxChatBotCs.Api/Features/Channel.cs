using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features;

public class Channel
{
    [JsonPropertyName("channel_id")] public string Id { get; init; }
    [JsonPropertyName("channel_name")] public string Name { get; init; }
    [JsonPropertyName("channel_type")] public ChannelTypeId Type { get; init; }
}