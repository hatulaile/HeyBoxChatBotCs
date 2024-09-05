using System.Reflection;
using HeyBoxChatBotCs.Api.Enums;
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
        =>
            Log.Info($"{Name} 正在重启!");


    //todo 命令部分未完成
    public virtual void OnRegisteringCommands()
    {
    }

    public virtual void OnUnregisteringCommands()
    {
    }

    public int CompareTo(IPlugin<IConfig>? other) => -Priority.CompareTo(other?.Priority);
}