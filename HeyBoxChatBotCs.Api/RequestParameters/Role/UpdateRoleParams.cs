using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Converters;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.RequestParameters.Role;

public class UpdateRoleParams
{
    public UpdateRoleParams(string id, CreateRoleParams createRoleParams)
    {
        Id = id;
        Name = createRoleParams.Name;
        Icon = createRoleParams.Icon;
        ColorList = createRoleParams.ColorList;
        RoomId = createRoleParams.RoomId;
        Type = createRoleParams.Type;
        PermissionStr = createRoleParams.PermissionsStr;
        Color = createRoleParams.Color;
        IsHoist = createRoleParams.IsHoist;
        Nonce = RequestAck.GetAckId();
    }

    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("icon")] public string? Icon { get; set; }
    [JsonPropertyName("color_list")] public long[]? ColorList { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
    [JsonPropertyName("type")] public RoleType Type { get; set; }
    [JsonPropertyName("permissions")] public string PermissionStr { get; set; }
    [JsonPropertyName("color")] public long? Color { get; set; }

    [JsonPropertyName("hoist"), JsonConverter(typeof(NumberBooleanJsonConverter))]
    public bool IsHoist { get; set; }

    [JsonPropertyName("nonce")] public string Nonce { get; set; }
}