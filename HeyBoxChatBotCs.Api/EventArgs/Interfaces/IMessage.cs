namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IMessage : IEvent
{
    
    public string MessageId { get; init; }
    public long SendTime { get; init; }
}