using HeyBoxChatBotCs.Api.Commands.ConsoleCommands;

namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public class ConsoleCommandHandler : CommandHandler
{
    public static ConsoleCommandHandler Create()
    {
        var consoleCommandHandler = new ConsoleCommandHandler();
        consoleCommandHandler.LoadOriginalCommand();
        return consoleCommandHandler;
    }

    protected override void LoadOriginalCommand()
    {
        RegisterCommand(new ConsoleHelpCommand());
        RegisterCommand(new StopServerCommand());
        RegisterCommand(new Test());
    }
}