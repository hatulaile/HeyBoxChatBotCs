using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestResponse;

internal class CreateRoleResponse
{
    [JsonPropertyName("role")] public required Role Role { get; init; }
}