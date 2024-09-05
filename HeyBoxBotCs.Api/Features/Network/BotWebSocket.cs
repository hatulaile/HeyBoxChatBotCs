using System.Net.WebSockets;

namespace HeyBoxBotCs.Api.Features.Network;

public class BotWebSocket : IDisposable
{
    private const int ACK_SLEEP_TIME = 25000;

    private const string QUERY = "client_type=heybox_chat&x_client_type=web&os_type=web&" +
                                 "x_os_type=bot&x_app=heybox_chat&chat_os_type=bot&chat_version=999.0.0";

    public readonly Uri WebsocketUri = new Uri("wss://chat.xiaoheihe.cn/chatroom/ws/connect");

    public BotWebSocket(Bot.Bot bot)
    {
        Bot = bot;
    }

    private Bot.Bot Bot { get; }


    private CancellationTokenSource WebSocketCts { get; } = new();

    public ClientWebSocket? WebSocket { get; private set; }

    public void Start()
    {
        if (WebSocket?.State is WebSocketState.Open)
        {
            Log.Warn("Websocket 已启动,请不要重复启动!");
            return;
        }

        try
        {
            WebSocket = new ClientWebSocket();

            WebSocket.Options.SetRequestHeader("token", Bot.Token);

            WebSocket.ConnectAsync(new Uri(WebsocketUri, QUERY), default);

            new Thread(ReceiveMessage)
            {
                IsBackground = true,
                Name = "Websocket Receive Message"
            }.Start();

            new Thread(Ack)
            {
                IsBackground = true,
                Name = "Ack keep Thread",
            }.Start(WebSocketCts);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }

    protected void ReceiveMessage(object? ctsObject)
    {
        if (ctsObject is not CancellationToken cts)
        {
            Log.Error("ReceiveMessage方法传值错误,程序中断!!!!");
            //todo 退出值带重设
            Environment.Exit(25000);
            return;
        }

        while (true)
        {
            if (cts.IsCancellationRequested)
            {
                Log.Debug("已取消接收信息进程!");
                break;
            }
        }
    }

    protected void Ack(object? ctsObject)
    {
        if (ctsObject is not CancellationToken cts)
        {
            Log.Error("Ack方法传值错误,程序中断!!!!");
            //todo 退出值带重设
            Environment.Exit(25000);
            return;
        }

        while (true)
        {
            if (cts.IsCancellationRequested)
            {
                Log.Debug("已取消心跳进程!");
                break;
            }

            if (WebSocket?.State is WebSocketState.Open)
            {
                Log.Debug("机器人正在发送心跳!");
                WebSocket.SendAsync("PING"u8.ToArray(), WebSocketMessageType.Text, true, default).Wait();
            }
            else
            {
                Log.Error($"机器人意外中断链接!正在尝试重新链接~ Info:{WebSocket?.CloseStatus ?? WebSocketCloseStatus.Empty}");
                Start();
            }

            Thread.Sleep(ACK_SLEEP_TIME);
        }
    }

    public void Dispose()
    {
        WebSocketCts.Cancel();
        WebSocket?.Dispose();
        WebSocketCts.Dispose();
    }
}