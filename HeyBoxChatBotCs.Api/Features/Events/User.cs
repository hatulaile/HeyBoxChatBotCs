using HeyBoxChatBotCs.Api.EventArgs.User;

namespace HeyBoxChatBotCs.Api.Features.Events;

public static class User
{
    public static Event<UserSendMessageEventArgs> UserSendMessage { get; } = new();
}