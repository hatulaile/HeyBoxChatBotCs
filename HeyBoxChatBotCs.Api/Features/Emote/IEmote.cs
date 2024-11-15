using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.Emote;

public interface IEmote
{
    public User CreatorUser { get; }
    public string EmoteRoomId { get; }
    public string Extension { get; }
    public string Path { get; }
    public long CreatorTime { get; }
    public EmojiTypeId Type { get; }
}