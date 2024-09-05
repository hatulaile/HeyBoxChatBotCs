using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features.Network;

namespace HeyBoxChatBotCs.Api.Features.Bot;

public static class BotHttpUri
{
    private static readonly Dictionary<BotAction, Uri> BotActionToUri = new()
    {
        [BotAction.SendMessage] = new Uri("https://chat.xiaoheihe.cn/chatroom/v2/channel_msg/send"),
    };

    public static IReadOnlyDictionary<BotAction, Uri> UriDictionary => BotActionToUri;


    public static Uri? GetUri(BotAction action)
    {
        if (!BotActionToUri.TryGetValue(action, out Uri? uri) || uri is null)
        {
            Log.Warn($"未获取到动作地址:{action},可能是还未完成此功能!");
            return default;
        }

        return uri;
    }
}

public class Bot
{
    public Bot(string id, string token)
    {
        Id = id;
        Token = token;
    }

    public string Id { get; private set; }
    public string Token { get; private set; }

    private BotWebSocket? BotWebSocket { get; set; }

    private CancellationTokenSource MainThreadCts { get; } = new();

    public bool IsRunning { get; private set; }

    /// <summary>
    /// 开启Bot 此方法会阻塞程序!
    /// </summary>
    public void Start()
    {
        BotWebSocket ??= new BotWebSocket(this);
        BotWebSocket.Start();
        new Loader.Loader().Run();
        IsRunning = true;
        while (!MainThreadCts.IsCancellationRequested)
        {
            Thread.Sleep(1000);
        }
    }

    public void Close()
    {
        if (!IsRunning || BotWebSocket is null)
        {
            Log.Warn("你都没开始如何结束此Bot!");
        }

        BotWebSocket?.Dispose();
        MainThreadCts.Cancel();
        MainThreadCts.Dispose();
    }

    //todo 未完成!!!!!!!
    public void SendMessage(string message)
    {
        if (!IsRunning)
        {
            Log.Warn("Bot 暂未启动,请启动后在使用此功能!");
            return;
        }
    }
}