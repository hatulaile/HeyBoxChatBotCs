using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotRequestDto;

public class DeleteRoomRoleDto
{
    public DeleteRoomRoleDto(Role role)
    {
        RoleId = role.Id;
        RoomId = role.RoomId;
    }

    public DeleteRoomRoleDto(string roleId, string roomId)
    {
        RoleId = roleId;
        RoomId = roomId;
    }

    [JsonPropertyName("role_id")] public string RoleId { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
}