namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public class UserCommandHandler : CommandHandler
{
    public static UserCommandHandler Create()
    {
        var consoleCommandHandler = new UserCommandHandler();
        consoleCommandHandler.LoadOriginalCommand();
        return consoleCommandHandler;
    }

    protected override void LoadOriginalCommand()
    {
    }
}