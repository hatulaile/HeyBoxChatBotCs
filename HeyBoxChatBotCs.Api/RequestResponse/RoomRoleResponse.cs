using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestResponse;

internal class RoomRoleResponse
{
    [JsonPropertyName("roles")] public required Role[] Roles { get; init; }
}