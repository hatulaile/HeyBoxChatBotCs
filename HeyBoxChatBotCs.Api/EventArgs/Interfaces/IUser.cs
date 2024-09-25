namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IUser : IEvent
{
    Features.User User { get; init; }
}