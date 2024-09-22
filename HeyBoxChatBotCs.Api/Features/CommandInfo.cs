using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features;

public class CommandInfo
{
    [JsonPropertyName("id")] public required string Id { get; init; }
    [JsonPropertyName("name")] public required string Message { get; init; }
    [JsonPropertyName("options")] public CommandOption[]? Options { get; init; }
    [JsonPropertyName("type")] public CommandTypeId Type { get; init; }
    [JsonIgnore] public List<CommandOption> AllOption => Options?.ToList() ?? [];
}

public class CommandOption
{
    [JsonPropertyName("name")] public required string Name { get; init; }
    [JsonPropertyName("type")] public CommandArgsTypeId Type { get; init; }
    [JsonPropertyName("value")] public required string Value { get; init; }
}