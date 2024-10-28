using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotActionResult;

public class UpdateRoleResult
{
    [JsonPropertyName("role")] public required Role Role { get; init; }
}