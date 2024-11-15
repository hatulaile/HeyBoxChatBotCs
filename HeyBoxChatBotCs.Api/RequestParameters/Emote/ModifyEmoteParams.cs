using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features.Emote;

namespace HeyBoxChatBotCs.Api.RequestParameters.Emoji;

public class ModifyEmoteParams
{
    public ModifyEmoteParams(IEmote emote, string name, string roomId) : this(emote.Path, name, roomId)
    {
    }

    public ModifyEmoteParams(string path, string name, string roomId)
    {
        Path = path;
        Name = name;
        RoomId = roomId;
    }

    [JsonPropertyName("path")] public string Path { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("room_id")] public string RoomId { get; set; }
}