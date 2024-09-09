using System.Reflection;
using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Exceptions;
using HeyBoxChatBotCs.Api.Interfaces;

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

namespace HeyBoxChatBotCs.Api.Features;

public abstract class Plugin<TConfig> : IPlugin<TConfig>
    where TConfig : IConfig, new()
{
    protected Plugin()
    {
        Assembly = Assembly.GetCallingAssembly();
        Name = Assembly.GetName().Name!;
        Author = Assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
        Version = Assembly.GetName().Version;
        Priority = PluginPriority.Default;
        Config = new TConfig();
    }

    public Assembly Assembly { get; }
    public virtual string Name { get; }
    public virtual string? Author { get; }
    public virtual PluginPriority Priority { get; }
    public virtual Version? Version { get; }

    public TConfig Config { get; }

    public Dictionary<Type, Dictionary<Type, ICommand>> Commands { get; } = new()
    {
        [typeof(ConsoleCommandHandler)] =
        {
        }
    };

    public virtual void OnEnabled()
    {
        AssemblyInformationalVersionAttribute? attribute =
            Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        Log.Info(
            $" {Author} 制作的插件 {Name} v{(Version is not null ? $"{Version.Major}.{Version.Minor}.{Version.Build}" : attribute is not null ? attribute.InformationalVersion : string.Empty)} 已启用");
    }

    public virtual void OnDisabled() =>
        Log.Info($"{Name} 正在禁用!");

    public virtual void OnReloaded()
        => Log.Info($"{Name} 正在重启!");


    public virtual void OnRegisteringCommands()
    {
        try
        {
            foreach (Type type in Assembly.GetTypes())
            {
                if (type.GetInterface("ICommand") != typeof(ICommand) ||
                    !type.IsDefined(typeof(CommandHandlerAttribute), true)) continue;
                foreach (CustomAttributeData data in type.GetCustomAttributesData()
                             .Where(data => data.AttributeType == typeof(CommandHandlerAttribute)))
                {
                    Type? key = (Type?)data.ConstructorArguments.ElementAt(0).Value;
                    if (key is not null &&
                        Commands.TryGetValue(key, out Dictionary<Type, ICommand>? dictionary))
                    {
                        if (!dictionary.TryGetValue(type, out ICommand? command))
                        {
                            command = (ICommand)Activator.CreateInstance(type)!;
                        }

                        try
                        {
                            if (key == typeof(ConsoleCommandHandler))
                            {
                                ConsoleCommandProcessor.ConsoleCommandHandler.RegisterCommand(command);
                            }
                        }
                        catch (CommandRegisteredException registeredException)
                        {
                            Log.Error(registeredException);
                            Log.Error($"注册命令时 {command.Command} 已注册,不知道为什么重复注册了!");
                        }
                        catch (ArgumentException argumentException)
                        {
                            Log.Error(argumentException);
                            Log.Error("注册命令遇到参数异常!");
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception);
                            Log.Error("注册命令遇到未知异常!");
                        }

                        Commands[key][type] = command;
                    }
                    else
                    {
                        Log.Warn($"发现未注册处理事件 {key?.Name ?? "NULL"},可能是未注册或者未实现!");
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception);
            Log.Error("注册命令遇到错误!");
        }
    }

    public virtual void OnUnregisteringCommands()
    {
        foreach (KeyValuePair<Type, ICommand> value in Commands.SelectMany(key => key.Value))
        {
            try
            {
                if (value.Key == typeof(ConsoleCommandHandler))
                {
                    ConsoleCommandProcessor.ConsoleCommandHandler.UnRegisterCommand(value.Value);
                }
            }
            catch (CommandUnregisteredException commandUnregisteredException)
            {
                Log.Error(commandUnregisteredException);
                Log.Error("取消注册命令时发生错误,命令还未注册!");
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                Log.Error("取消注册命令遇到未知错误!");
            }
        }
    }

    public int CompareTo(IPlugin<IConfig>? other) => -Priority.CompareTo(other?.Priority);
}