using HeyBoxBotCs.Api.EventArgs.User;

namespace HeyBoxBotCs.Api.Features.Events;

public static class User
{
    public static Event<UserSendMessageEventArgs> UserSendMessage { get; } = new();
}