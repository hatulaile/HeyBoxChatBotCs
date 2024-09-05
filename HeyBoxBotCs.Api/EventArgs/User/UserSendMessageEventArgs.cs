using HeyBoxBotCs.Api.EventArgs.Interfaces;

namespace HeyBoxBotCs.Api.EventArgs.User;

public class UserSendMessageEventArgs : IUserEvent
{
    public Features.User User { get; }
}