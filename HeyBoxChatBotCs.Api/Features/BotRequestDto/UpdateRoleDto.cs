using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.BotRequestDto;

public class UpdateRoleDto
{
    public UpdateRoleDto(string id, CreateRoleDto createRoleDto)
    {
        Id = id;
        Name = createRoleDto.Name;
        Icon = createRoleDto.Icon;
        ColorList = createRoleDto.ColorList;
        RoomId = createRoleDto.RoomId;
        Type = createRoleDto.Type;
        PermissionStr = createRoleDto.PermissionsStr;
        Color = createRoleDto.Color;
        Hoist = createRoleDto.Hoist;
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
    [JsonPropertyName("hoist")] public int Hoist { get; set; }
    [JsonPropertyName("nonce")] public string Nonce { get; set; }
}