using HeyBoxChatBotCs.Api.Features.Bot;

namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IBot : IEvent
{
    public long BotId { get; init; }
}