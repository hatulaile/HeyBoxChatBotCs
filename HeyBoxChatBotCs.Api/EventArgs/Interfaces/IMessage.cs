namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IMessage : IEvent
{
    string MessageId { get; init; }
    long SendTime { get; init; }
}