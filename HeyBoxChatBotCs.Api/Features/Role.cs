using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features;

public class Role
{
    [JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("icon")] public required Uri Icon { get; set; }
    [JsonPropertyName("color_list")] public required long[]? ColorList { get; set; }
    [JsonPropertyName("id")] public required string Id { get; set; }
    [JsonPropertyName("room_id")] public required string RoomId { get; set; }

    [JsonPropertyName("permissions")]
    public required string PermissionStr
    {
        get => Permission.ToString("D");
        init => Permission = Enum.Parse<Permission>(value);
    }

    [JsonPropertyName("type")] public required RoleType Type { get; set; }
    [JsonPropertyName("color")] public required long Color { get; set; }
    [JsonPropertyName("position")] public required int Position { get; set; }
    [JsonPropertyName("del_tag")] public required int DelTag { get; set; }
    [JsonPropertyName("hoist")] public required int Hoist { get; set; }
    [JsonPropertyName("creator")] public required int Creator { get; set; }
    [JsonPropertyName("create_time")] public required long CreateTime { get; set; }

    [JsonPropertyName("mentionable")] public int? Mentionable { get; set; }
    [JsonIgnore] public bool IsExist => DelTag == 1;
    [JsonIgnore] public bool IsHoist => Hoist == 1;
    [JsonIgnore] public bool IsCreator => Creator == 0;
    [JsonIgnore] public bool CanMention => Mentionable == 1;
    [JsonIgnore] public Permission Permission { get; set; }

    [Obsolete("文档标记弃用"), JsonPropertyName("department_id")]
    public string? DepartmentId { get; set; }
}