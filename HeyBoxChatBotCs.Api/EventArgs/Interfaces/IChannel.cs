using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IChannel : IEvent
{
    public Channel Channel { get; init; }
}