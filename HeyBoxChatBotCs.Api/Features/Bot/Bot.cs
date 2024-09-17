using System.Text.Json;
using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features.Message;
using HeyBoxChatBotCs.Api.Features.Network;

namespace HeyBoxChatBotCs.Api.Features.Bot;

public class Bot
{
    public static Bot? Instance { get; private set; }

    public Bot(string id, string token)
    {
        if (Instance is not null)
        {
            //todo 异常待制作
            throw new Exception("实例已存在,请不要重复新建机器人,如果要多机器人请新开一个应用!");
        }

        Id = id;
        Token = token;
        Instance = this;
    }

    public string Id { get; private set; }
    public string Token { get; private set; }

    private BotWebSocket? BotWebSocket { get; set; }

    public bool IsRunning { get; private set; }

    public static event EventHandler? BotStart;

    public static event EventHandler? BotClose;

    /// <summary>
    /// 开启Bot 此方法会阻塞程序!
    /// </summary>
    public void Start()
    {
        BotWebSocket ??= new BotWebSocket(this);
        BotWebSocket.Start();
        new Loader.Loader().Run();
        IsRunning = true;
        BotStart?.Invoke(this);
        Log.Info("控制台命令已启用,可以输入!");
        new Thread(ConsoleCommandProcessor.Run)
        {
            IsBackground = true,
            Name = "Console ReadLine Thread"
        }.Start();
        while (IsRunning)
        {
            Thread.Sleep(1000);
        }
    }

    public void Close()
    {
        if (!IsRunning || BotWebSocket is null)
        {
            Log.Warn("你还未启用Bot,无法关闭!");
        }

        ConsoleCommandProcessor.ConsoleReadCts?.Cancel();
        BotClose?.Invoke(this);
        IsRunning = false;
        BotWebSocket?.Dispose();
        Log.Info("已关闭Bot,即将退出程序!");
        Misc.Misc.Exit(0);
    }

    protected async Task<T?> BotSendAction<T>(object body, BotAction action)
    {
        if (!BotRequestUrl.TryGetUri(action, out Uri? uri))
        {
            Log.Error($"Bot发送{action.ToString()}时未找到URI!");
            return default;
        }

        Log.Debug("BOT动作的JSON为:" + body);
        return await HttpRequest.Post<T>(uri!, JsonSerializer.Serialize(body), new Dictionary<string, string>()
        {
            { "token", Token }
        });
    }

    public async void SendMessage(MessageBase message)
    {
        if (!IsRunning)
        {
            Log.Warn("Bot 暂未启动,请启动后在使用此功能!");
            return;
        }

        try
        {
            SendMessageResult? result = await BotSendAction<SendMessageResult>(message, BotAction.SendMessage);
            if (result is null)
            {
                Log.Error("发送信息后返回的数据为空!");
                return;
            }

            Log.Debug($"发送信息 \"{result.Message}\" 成功,状态:{result.Status}.");
        }
        catch (Exception exception)
        {
            Log.Error("发送信息时出错:" + exception);
        }
    }
}