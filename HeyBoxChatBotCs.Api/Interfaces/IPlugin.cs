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
    void OnDisabled();
    void OnReloaded();
    void OnRegisteringCommands();
    void OnUnregisteringCommands();
}