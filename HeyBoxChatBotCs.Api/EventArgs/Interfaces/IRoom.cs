using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IRoom : IEvent
{
    public Room Room { get; init; }
}