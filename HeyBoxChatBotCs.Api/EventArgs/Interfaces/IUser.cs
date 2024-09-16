namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IUser : IEvent
{
    public Features.User User { get; init; }
}