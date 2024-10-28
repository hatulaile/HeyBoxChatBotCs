using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotActionResult;

public class OperationUserRoleResult
{
    [JsonPropertyName("user")] public required RoleUser User { get; init; }

    public class RoleUser
    {
        [JsonPropertyName("roles")] public required string[] Roles { get; init; }
        [JsonPropertyName("user_id")] public required string UserId { get; init; }
        [JsonPropertyName("room_id")] public required string RoomId { get; init; }


        [JsonPropertyName("department_id"), Obsolete($"文档标记弃用,与{nameof(RoomId)}内容一致.")]
        public string? DepartmentId { get; init; }
    }
}