using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.RequestParameters.Role;

public class DeleteRoleParams
{
    public DeleteRoleParams(Features.Role role)
    {
        RoleId = role.Id;
        RoomId = role.RoomId;
    }

    public DeleteRoleParams(string roleId, string roomId)
    {
        RoleId = roleId;
        RoomId = roomId;
    }

    [JsonPropertyName("role_id")] public string RoleId { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
}