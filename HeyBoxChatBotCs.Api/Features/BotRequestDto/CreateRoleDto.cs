using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.BotRequestDto;

public class CreateRoleDto
{
    public CreateRoleDto(string name, string roomId, Permission permission, bool isHoist,
        RoleType type = RoleType.Default,
        string? icon = null, long[]? colorList = null, int? color = null, string? nonce = null)
    {
        Name = name;
        RoomId = roomId;
        PermissionsStr = permission.ToString("D");
        Type = type;
        Hoist = isHoist ? 1 : 0;
        Icon = icon;
        ColorList = colorList;
        Color = color;
        Nonce = nonce ?? RequestAck.GetAckId();
    }

    public CreateRoleDto(string roomId, Role role)
    {
        RoomId = roomId;
        Name = role.Name;
        Icon = role.Icon.ToString();
        ColorList = role.ColorList;
        PermissionsStr = role.PermissionStr;
        Type = role.Type;
        Color = role.Color;
        Hoist = role.Hoist;
        Nonce = RequestAck.GetAckId();
    }

    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("icon")] public string? Icon { get; set; }
    [JsonPropertyName("color_list")] public long[]? ColorList { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
    [JsonPropertyName("permissions")] public string PermissionsStr { get; set; }
    [JsonPropertyName("type")] public RoleType Type { get; set; }
    [JsonPropertyName("color")] public long? Color { get; set; }
    [JsonPropertyName("hoist")] public int Hoist { get; set; }
    [JsonPropertyName("nonce")] public string Nonce { get; set; }

    public static explicit operator CreateRoleDto(Role role)
        => new(role.Name, role.RoomId, role.Permission, role.IsHoist, role.Type, role.Icon.ToString());
}