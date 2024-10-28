using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.User;

namespace HeyBoxChatBotCs.Api.Events;

public static class User
{
    public static Event<UserSendCommandData> UserSendCommand { get; set; } = new();

    public static Event<UserOperationMessageEmojiData> UserOperationMessageEmoji { get; set; } = new();

    public static Event<UserOperationMessageEmojiData> UserAddMessageEmoji { get; set; } = new();

    public static Event<UserOperationMessageEmojiData> UserDeleteMessageEmoji { get; set; } = new();

    public static async Task OnUserSendCommandAsync(UserSendCommandData ev)
    {
        await UserSendCommand.InvokeAsync(ev);
    }

    public static async Task OnUserOperationMessageEmoji(UserOperationMessageEmojiData ev)
    {
        await UserOperationMessageEmoji.InvokeAsync(ev);
    }

    public static async Task OnUserAddMessageEmoji(UserOperationMessageEmojiData ev)
    {
        await UserAddMessageEmoji.InvokeAsync(ev);
    }

    public static async Task OnUserDeleteMessageEmoji(UserOperationMessageEmojiData ev)
    {
        await UserDeleteMessageEmoji.InvokeAsync(ev);
    }
}