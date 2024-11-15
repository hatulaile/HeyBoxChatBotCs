using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestResponse;

internal class ModifyRoleResponse
{
    [JsonPropertyName("role")] public required Role Role { get; init; }
}