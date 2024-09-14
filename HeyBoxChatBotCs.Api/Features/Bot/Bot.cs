using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Enums;
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
        while (true)
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

        BotClose?.Invoke(this);
        IsRunning = false;
        BotWebSocket?.Dispose();
        Log.Info("已关闭Bot,即将退出程序!");
        Misc.Misc.Exit(0);
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