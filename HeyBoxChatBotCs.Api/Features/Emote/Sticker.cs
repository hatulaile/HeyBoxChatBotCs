using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.Emote;

public class Sticker : IEmote
{
    public Sticker(User creatorUser, string emoteRoomId, string extension, string path, long creatorTime)
    {
        CreatorUser = creatorUser;
        EmoteRoomId = emoteRoomId;
        Extension = extension;
        Path = path;
        CreatorTime = creatorTime;
    }

    public User CreatorUser { get; }
    public string EmoteRoomId { get; }
    public string Extension { get; }
    public string Path { get; }
    public long CreatorTime { get; }
    public EmojiTypeId Type => EmojiTypeId.Sticker;
    public override string ToString() => $"[custom{EmoteRoomId}_{Path}.{Extension}]";
}