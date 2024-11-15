using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.Network;

public class BotWebSocket : IDisposable
{
    private const int ERROR_SLEEP_TIME = 30000;

    private const int ACK_SLEEP_TIME = 25000;

    private const int MAX_BUFFER_SIZE = 1024;

    protected readonly ArraySegment<byte> AckSendMessage = "PING"u8.ToArray();


    //private const string QUERY = "?chat_os_type=bot&client_type=heybox_chat&chat_version=1.27.2";
    //"client_type=heybox_chat&x_client_type=web&os_type=web&" +
    // "x_os_type=bot&x_app=heybox_chat&chat_os_type=bot&chat_version=999.0.0";


    public BotWebSocket(Bot bot)
    {
        Bot = bot;
    }

    private Bot Bot { get; }


    private CancellationTokenSource? WebSocketCts { get; set; }

    public ClientWebSocket? WebSocket { get; private set; }

    public void Dispose()
    {
        ReceiveMessage -= ServerMessageHandler.System.ServerMessageHandler.ProcessMessageAsync;
        WebSocketCts?.Cancel();
        WebSocketCts?.Dispose();
        WebSocket?.Dispose();
    }

    public async Task Start()
    {
        if (WebSocket?.State is WebSocketState.Open)
        {
            Log.Warn("Websocket 已启动,请不要重复启动!");
            return;
        }

        try
        {
            if (WebSocketCts is { IsCancellationRequested: false })
            {
                await WebSocketCts.CancelAsync().ConfigureAwait(false);
                WebSocketCts?.Dispose();
            }

            WebSocketCts = new CancellationTokenSource();

            WebSocket = new ClientWebSocket();
            WebSocket.Options.SetRequestHeader("token", Bot.Token);
            await WebSocket.ConnectAsync(
                BotRequestUrl.GetUri(BotOperations.Connect),
                WebSocketCts.Token).ConfigureAwait(false);

            _ = ReceiveServerMessage();

            ReceiveMessage += ServerMessageHandler.System.ServerMessageHandler.ProcessMessageAsync;


            _ = Ack();
        }
        catch (Exception e)
        {
            Log.Error(e);

            Dispose();
            Log.Error($"连接错误,将于{ERROR_SLEEP_TIME}毫秒后重连!");
            Thread.Sleep(ERROR_SLEEP_TIME);
            await Start().ConfigureAwait(false);
        }
    }


    public event ReceiveMessage? ReceiveMessage;

    protected async Task ReceiveServerMessage()
    {
        var buffer = new byte[MAX_BUFFER_SIZE];
        var sb = new StringBuilder();
        while (true)
        {
            if (WebSocketCts is null || WebSocketCts.IsCancellationRequested || WebSocket is null)
            {
                Log.Debug("已取消接收信息进程!");
                return;
            }

            try
            {
                while (true)
                {
                    WebSocketReceiveResult ret =
                        await WebSocket.ReceiveAsync(buffer, WebSocketCts.Token).ConfigureAwait(false);
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
            catch (TaskCanceledException)
            {
                Log.Debug("接收信息被取消!");
                return;
            }
            catch (SocketException socketException)
            {
                Log.Error(socketException);
                Log.Error($"机器人意外中断链接!正在尝试重新链接~ Info:{WebSocket?.CloseStatus ?? WebSocketCloseStatus.Empty}");
                _ = Start();
                return;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                Log.Error($"机器人意外中断链接!正在尝试重新链接~ Info:{WebSocket?.CloseStatus ?? WebSocketCloseStatus.Empty}");
                _ = Start();
                return;
            }
        }
    }

    protected async Task Ack()
    {
        await Task.Delay(ACK_SLEEP_TIME).ConfigureAwait(false);

        while (true)
        {
            if (WebSocketCts is null || WebSocketCts.IsCancellationRequested)
            {
                Log.Debug("已取消心跳进程!");
                break;
            }

            if (WebSocket?.State is WebSocketState.Open)
            {
                Log.Debug("机器人正在发送心跳!");
                await WebSocket.SendAsync(AckSendMessage, WebSocketMessageType.Text, true, WebSocketCts.Token)
                    .ConfigureAwait(false);
            }
            else
            {
                if (WebSocketCts.IsCancellationRequested)
                {
                    break;
                }

                Log.Error($"机器人意外中断链接!正在尝试重新链接~ Info:{WebSocket?.CloseStatus ?? WebSocketCloseStatus.Empty}");
                _ = Start();
                return;
            }

            await Task.Delay(ACK_SLEEP_TIME).ConfigureAwait(false);
        }
    }
}