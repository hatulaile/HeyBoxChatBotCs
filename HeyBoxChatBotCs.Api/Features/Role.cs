using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Converters;

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

    [JsonPropertyName("del_tag"), JsonConverter(typeof(NumberBooleanJsonConverter))]
    public required bool IsExist { get; set; }

    [JsonPropertyName("hoist"), JsonConverter(typeof(NumberBooleanJsonConverter))]
    public required bool IsHoist { get; set; }

    [JsonPropertyName("mentionable"), JsonConverter(typeof(NumberBooleanJsonConverter))]
    public bool CanMentionable { get; set; }

    [JsonPropertyName("creator")] public required ulong Creator { get; set; }

    [JsonPropertyName("create_time")] public required long CreateTime { get; set; }

    [JsonIgnore] public Permission Permission { get; set; }

    [Obsolete("文档标记弃用"), JsonPropertyName("department_id")]
    public string? DepartmentId { get; set; }
}