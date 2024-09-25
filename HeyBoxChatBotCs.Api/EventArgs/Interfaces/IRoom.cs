using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IRoom : IEvent
{
    Room Room { get; init; }
}