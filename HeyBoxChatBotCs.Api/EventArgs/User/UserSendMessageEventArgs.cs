using HeyBoxChatBotCs.Api.EventArgs.Interfaces;

namespace HeyBoxChatBotCs.Api.EventArgs.User;

public class UserSendMessageEventArgs : IUserEvent
{
    public Features.User User { get; }
}