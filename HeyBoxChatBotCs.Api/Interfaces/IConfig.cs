using System.ComponentModel;

namespace HeyBoxChatBotCs.Api.Interfaces;

public interface IConfig
{
    [Description("是否启用此插件")] bool IsEnabled { get; set; }
    [Description("是否启用调试模式")] bool IsDebug { get; set; }
}