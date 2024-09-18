using System.Net.WebSockets;
using System.Text;
using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Features.Network;

public class BotWebSocket : IDisposable
{
    private const int ERROR_SLEEP_TIME = 30000;

    private const int ACK_SLEEP_TIME = 25000;

    private const int MAX_BUFFER_SIZE = 1024;


    //private const string QUERY = "?chat_os_type=bot&client_type=heybox_chat&chat_version=1.27.2";
    //"client_type=heybox_chat&x_client_type=web&os_type=web&" +
    // "x_os_type=bot&x_app=heybox_chat&chat_os_type=bot&chat_version=999.0.0";


    public BotWebSocket(Bot.Bot bot)
    {
        Bot = bot;
    }

    public bool IsRunning { get; private set; }

    private Bot.Bot Bot { get; }


    private CancellationTokenSource? WebSocketCts { get; set; }

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
            IsRunning = true;

            WebSocketCts = new CancellationTokenSource();

            WebSocket = new ClientWebSocket();

            WebSocket.Options.SetRequestHeader("token", Bot.Token);

            WebSocket.ConnectAsync(
                    BotRequestUrl.GetUri(BotAction.Connect)!,
                    CancellationToken.None)
                .Wait();

            new Thread(ReceiveServerMessage)
            {
                IsBackground = true,
                Name = "Websocket Receive Message"
            }.Start(WebSocketCts);

            ReceiveMessage += ServerMessageHandler.System.ServerMessageHandler.ProcessMessage;

            new Thread(Ack)
            {
                IsBackground = true,
                Name = "Ack keep Thread",
            }.Start(WebSocketCts);
        }
        catch (Exception e)
        {
            Log.Error(e);

            Dispose();
            Log.Error($"连接错误,将于{ERROR_SLEEP_TIME}毫秒后重连!");
            Thread.Sleep(ERROR_SLEEP_TIME);
        }
    }


    public event ReceiveMessage? ReceiveMessage;

    protected void ReceiveServerMessage(object? ctsObject)
    {
        if (ctsObject is not CancellationTokenSource cts)
        {
            Log.Error("ReceiveMessage方法传值错误,程序中断!!!!");
            //todo 退出值带重设
            Misc.Misc.Exit(25000);
            return;
        }

        byte[] buffer = new byte[MAX_BUFFER_SIZE];
        StringBuilder sb = new StringBuilder();
        while (true)
        {
            if (cts.IsCancellationRequested || WebSocket is null)
            {
                Log.Debug("已取消接收信息进程!");
                return;
            }

            try
            {
                while (true)
                {
                    var ret = WebSocket.ReceiveAsync(buffer, WebSocketCts?.Token ?? default).Result;
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, ret.Count));
                    if (ret.EndOfMessage)
                    {
                        break;
                    }
                }

                if (sb.ToString() is "PONG")
                {
                    Log.Debug("已接受到服务器心跳返回信息!");
                }
                else
                {
                    Log.Debug("已接受到服务器信息:" + sb);
                    ReceiveMessage?.Invoke(sb.ToString());
                }

                sb.Clear();
            }
            catch (ObjectDisposedException objectDisposedException)
            {
                if (!IsRunning)
                {
                    break;
                }

                if (!cts.IsCancellationRequested)
                {
                    Log.Debug("主动退出Websocket链接");
                    return;
                }

                Log.Error(objectDisposedException);
                Log.Error($"机器人意外中断链接!正在尝试重新链接~ Info:{WebSocket?.CloseStatus ?? WebSocketCloseStatus.Empty}");
                Dispose();
                Start();
            }
            catch (Exception exception)
            {
                if (!IsRunning)
                {
                    break;
                }

                Log.Error(exception);
                Log.Error($"机器人意外中断链接!正在尝试重新链接~ Info:{WebSocket?.CloseStatus ?? WebSocketCloseStatus.Empty}");
                Dispose();
                Start();
            }
        }
    }

    protected readonly ArraySegment<byte> AckSendMessage = "PING"u8.ToArray();

    protected void Ack(object? ctsObject)
    {
        if (ctsObject is not CancellationTokenSource cts)
        {
            Log.Error("Ack方法传值错误,程序中断!!!!");
            //todo 退出值带重设
            Misc.Misc.Exit(25000);
            return;
        }

        Thread.Sleep(ACK_SLEEP_TIME);

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
                WebSocket.SendAsync(AckSendMessage, WebSocketMessageType.Text, true, default).Wait();
            }
            else
            {
                if (!IsRunning || cts.IsCancellationRequested)
                {
                    break;
                }

                Log.Error($"机器人意外中断链接!正在尝试重新链接~ Info:{WebSocket?.CloseStatus ?? WebSocketCloseStatus.Empty}");
                cts.Cancel();
                Start();
            }

            Thread.Sleep(ACK_SLEEP_TIME);
        }
    }

    public void Dispose()
    {
        IsRunning = false;
        ReceiveMessage -= ServerMessageHandler.System.ServerMessageHandler.ProcessMessage;
        WebSocketCts?.Cancel();
        WebSocket?.Dispose();
        WebSocketCts?.Dispose();
    }
}