using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.User;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataHandlers.User;

public class UserOperationMessageEmojiHandler : IDataHandler
{
    public async Task ProcessDataAsync(object? serverMessage)
    {
        if (serverMessage is not UserOperationMessageEmojiData userOperationMessageEmojiData)
        {
            return;
        }

        await Events.User.OnUserOperationMessageEmoji(userOperationMessageEmojiData);
        if (userOperationMessageEmojiData.ReactionActionType == ReactionActionType.Add)
        {
            await Events.User.OnUserAddMessageEmoji(userOperationMessageEmojiData);
        }
        else if (userOperationMessageEmojiData.ReactionActionType == ReactionActionType.Delete)
        {
            await Events.User.OnUserDeleteMessageEmoji(userOperationMessageEmojiData);
        }
    }
}