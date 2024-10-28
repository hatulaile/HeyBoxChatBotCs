using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotActionResult;

public class RoomRoleResult
{
    [JsonPropertyName("roles")] public required Role[] Roles { get; init; }
}