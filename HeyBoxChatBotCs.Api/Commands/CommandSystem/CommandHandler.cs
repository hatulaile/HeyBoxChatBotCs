using System.Diagnostics.CodeAnalysis;
using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Exceptions;

namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public abstract class CommandHandler
{
    protected readonly Dictionary<string, string> CommandAliases = new(StringComparer.OrdinalIgnoreCase);
    protected readonly Dictionary<string, ICommandBase> Commands = new(StringComparer.OrdinalIgnoreCase);

    public virtual IEnumerable<ICommandBase> AllCommand => Commands.Values;

    public virtual bool TryGetCommand(string query, [NotNullWhen(true)] out ICommandBase? command)
    {
        if (CommandAliases.TryGetValue(query, out string? str))
        {
            query = str;
        }

        return Commands.TryGetValue(query, out command);
    }

    public virtual void RegisterCommand(ICommandBase commandBase)
    {
        if (string.IsNullOrWhiteSpace(commandBase.Command))
            throw new ArgumentException("此命令为空:" + commandBase.GetType().Name);
        if (!Commands.TryAdd(commandBase.Command, commandBase))
        {
            throw new CommandRegisteredException();
        }

        if (commandBase.Aliases is null)
            return;
        foreach (string alias in commandBase.Aliases)
        {
            if (!string.IsNullOrWhiteSpace(alias))
                CommandAliases.Add(alias, commandBase.Command);
        }
    }

    public virtual void UnRegisterCommand(ICommandBase commandBase)
    {
        if (!AllCommand.Contains(commandBase))
        {
            throw new CommandUnregisteredException(commandBase);
        }

        UnRegisterCommand(commandBase.Command);
    }

    public virtual void UnRegisterCommand(string query)
    {
        if (CommandAliases.TryGetValue(query, out string? str))
        {
            query = str;
        }

        if (!Commands.TryGetValue(query, out ICommandBase? command))
        {
            throw new CommandUnregisteredException(query);
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

    public virtual void ClearCommand()
    {
        Commands.Clear();
        CommandAliases.Clear();
    }

    protected abstract void LoadOriginalCommand();
}