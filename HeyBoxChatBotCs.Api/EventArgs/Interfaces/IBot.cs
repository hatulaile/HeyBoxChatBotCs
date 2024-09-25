namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IBot : IEvent
{
    long BotId { get; init; }
}