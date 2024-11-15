using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features.Emote;

namespace HeyBoxChatBotCs.Api.RequestParameters.Emoji;

public class DeleteEmoteParams
{
    public DeleteEmoteParams(IEmote emote, string roomId) : this(emote.Path, roomId)
    {
    }

    public DeleteEmoteParams(string path, string roomId)
    {
        Path = path;
        RoomId = roomId;
    }

    [JsonPropertyName("path")] public string Path { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
}