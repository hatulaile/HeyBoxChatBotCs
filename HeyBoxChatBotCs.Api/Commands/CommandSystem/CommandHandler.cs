using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Exceptions;

namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public abstract class CommandHandler
{
    protected readonly Dictionary<string, ICommand> Commands = new(StringComparer.OrdinalIgnoreCase);
    protected readonly Dictionary<string, string> CommandAliases = new(StringComparer.OrdinalIgnoreCase);

    public virtual IEnumerable<ICommand> AllCommand => Commands.Values;

    public virtual bool TryGetCommand(string query, out ICommand? command)
    {
        if (CommandAliases.TryGetValue(query, out string? str))
        {
            query = str;
        }

        return Commands.TryGetValue(query, out command);
    }

    public virtual void RegisterCommand(ICommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Command))
            throw new ArgumentException("此命令为空:" + command.GetType().Name);
        if (Commands.ContainsKey(command.Command))
        {
            throw new CommandRegisteredException();
        }

        Commands.Add(command.Command, command);
        if (command.Aliases is null)
            return;
        foreach (string alias in command.Aliases)
        {
            if (!string.IsNullOrWhiteSpace(alias))
                CommandAliases.Add(alias, command.Command);
        }
    }

    public virtual void UnRegisterCommand(ICommand command)
    {
        if (!AllCommand.Contains(command))
        {
            throw new CommandUnregisteredException(command);
        }

        UnRegisterCommand(command.Command);
    }

    public virtual void UnRegisterCommand(string query)
    {
        if (CommandAliases.TryGetValue(query, out string? str))
        {
            query = str;
        }

        if (!Commands.TryGetValue(query, out ICommand? command))
        {
            throw new CommandUnregisteredException(command: query);
        }

        Commands.Remove(command.Command);
        if (command.Aliases is null)
        {
            return;
        }

        foreach (string alias in command.Aliases)
        {
            CommandAliases.Remove(alias);
        }
    }

    public virtual void ClearCommadn()
    {
        Commands.Clear();
        CommandAliases.Clear();
    }

    protected abstract void LoadOriginalCommand();
}