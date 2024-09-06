using System.Reflection;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Interfaces;

public interface IPlugin<out TConfig> : IComparable<IPlugin<IConfig>>
    where TConfig : IConfig
{
    Assembly Assembly { get; }
    string Name { get; }
    string? Author { get; }
    PluginPriority Priority { get; }
    Version? Version { get; }
    TConfig Config { get; }
    void OnEnabled();

    /// <summary>
    /// 未实现
    /// </summary>
    void OnDisabled();

    /// <summary>
    /// 未实现
    /// </summary>
    void OnReloaded();

    /// <summary>
    /// 未实现
    /// </summary>
    void OnRegisteringCommands();

    /// <summary>
    /// 未实现
    /// </summary>
    void OnUnregisteringCommands();
}