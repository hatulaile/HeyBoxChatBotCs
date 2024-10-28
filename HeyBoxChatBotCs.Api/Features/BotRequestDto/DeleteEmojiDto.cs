using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotRequestDto;

public class DeleteEmojiDto
{
    public DeleteEmojiDto(string path, string roomId)
    {
        Path = path;
        RoomId = roomId;
    }

    [JsonPropertyName("path")] public string Path { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
}