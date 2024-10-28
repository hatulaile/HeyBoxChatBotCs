using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotActionResult;

public class CreateRoleResult
{
    [JsonPropertyName("role")] public required Role Role { get; init; }
}