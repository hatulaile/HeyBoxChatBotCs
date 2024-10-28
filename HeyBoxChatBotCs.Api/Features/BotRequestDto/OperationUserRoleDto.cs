using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotRequestDto;

public class OperationUserRoleDto
{
    public OperationUserRoleDto(long userId, Role role)
    {
        UserId = userId;
        RoleId = role.Id;
        RoomId = role.RoomId;
    }

    public OperationUserRoleDto(long userId, string roleId, string roomId)
    {
        UserId = userId;
        RoleId = roleId;
        RoomId = roomId;
    }

    [JsonPropertyName("to_user_id")] public long UserId { get; set; }
    [JsonPropertyName("role_id")] public string RoleId { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
}