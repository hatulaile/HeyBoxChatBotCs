using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.Interfaces;

namespace TestPlugin;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool IsDebug { get; set; }
}

public class TestPlugin : Plugin<Config>
{
    public override string Author => "hatu";
    public override string Name => "testPlugin";

    public override Version Version => new Version(0, 0, 1);
    public override void OnEnabled()
    {
        Log.Info($"{Name} 已开始运行哦!");
        base.OnEnabled();
    }
}