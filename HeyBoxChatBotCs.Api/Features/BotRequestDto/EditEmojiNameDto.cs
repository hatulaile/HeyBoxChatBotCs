using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotRequestDto;

public class EditEmojiNameDto
{
    public EditEmojiNameDto(string path, string name, string roomId)
    {
        Path = path;
        Name = name;
        RoomId = roomId;
    }

    [JsonPropertyName("path")] public string Path { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
}