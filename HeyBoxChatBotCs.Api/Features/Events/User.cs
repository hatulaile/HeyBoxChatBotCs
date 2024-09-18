using HeyBoxChatBotCs.Api.EventArgs.User;

namespace HeyBoxChatBotCs.Api.Features.Events;

public static class User
{
    public static Event<UserSendCommandEventArgs> UserSendCommand { get; set; } = new();

    public static void OnUserSendCommand(UserSendCommandEventArgs ev)
    {
        UserSendCommand.Invoke(ev);
    }
}