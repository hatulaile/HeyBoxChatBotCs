using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Converters;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestParameters.Role;

public class CreateRoleParams
{
    public CreateRoleParams(string name, string roomId, Permission permission, bool isHoist,
        RoleType type = RoleType.Default,
        string? icon = null, long[]? colorList = null, int? color = null, string? nonce = null)
    {
        Name = name;
        RoomId = roomId;
        PermissionsStr = permission.ToString("D");
        Type = type;
        IsHoist = isHoist;
        Icon = icon;
        ColorList = colorList;
        Color = color;
        Nonce = nonce ?? RequestAck.GetAckId();
    }

    public CreateRoleParams(string roomId, Features.Role role)
    {
        RoomId = roomId;
        Name = role.Name;
        Icon = role.Icon.ToString();
        ColorList = role.ColorList;
        PermissionsStr = role.PermissionStr;
        Type = role.Type;
        Color = role.Color;
        IsHoist = role.IsHoist;
        Nonce = RequestAck.GetAckId();
    }

    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("icon")] public string? Icon { get; set; }
    [JsonPropertyName("color_list")] public long[]? ColorList { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
    [JsonPropertyName("permissions")] public string PermissionsStr { get; set; }
    [JsonPropertyName("type")] public RoleType Type { get; set; }
    [JsonPropertyName("color")] public long? Color { get; set; }

    [JsonPropertyName("hoist"), JsonConverter(typeof(NumberBooleanJsonConverter))]
    public bool IsHoist { get; set; }

    [JsonPropertyName("nonce")] public string Nonce { get; set; }

    public static explicit operator CreateRoleParams(Features.Role role)
        => new(role.Name, role.RoomId, role.Permission, role.IsHoist, role.Type, role.Icon.ToString());
}