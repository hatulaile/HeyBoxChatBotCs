using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.RequestParameters.Role;

public class ModifyRoleParams
{
    public ModifyRoleParams(long userId, Features.Role role)
    {
        UserId = userId;
        RoleId = role.Id;
        RoomId = role.RoomId;
    }

    public ModifyRoleParams(long userId, string roleId, string roomId)
    {
        UserId = userId;
        RoleId = roleId;
        RoomId = roomId;
    }

    [JsonPropertyName("to_user_id")] public long UserId { get; set; }
    [JsonPropertyName("role_id")] public string RoleId { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
}