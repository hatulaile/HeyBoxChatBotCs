using System.Text.Encodings.Web;
using System.Text.Json;
using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features.Bot.BotActionResult;
using HeyBoxChatBotCs.Api.Features.Message;
using HeyBoxChatBotCs.Api.Features.Network;
using HeyBoxChatBotCs.Api.Features.Network.HttpBody;

namespace HeyBoxChatBotCs.Api.Features.Bot;

public class Bot
{
    public static readonly JsonSerializerOptions BotActionJsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

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
        Misc.Misc.Exit();
    }

    protected async Task<HttpResponseMessageValue<T?>?> BotSendAction<T>(object body, BotAction action,
        string contentType = "application/json")
    {
        if (!BotRequestUrl.TryGetUri(action, out Uri? uri))
        {
            Log.Error($"Bot发送{action.ToString()}时未找到URI!");
            return default;
        }

        string json = JsonSerializer.Serialize(body, BotActionJsonSerializerOptions);
        Log.Debug("BOT动作的JSON为:" + json);
        return await HttpRequest.Post<T>(uri!, new RawBody(json, contentType), new Dictionary<string, string>
        {
            { "token", Token }
        });
    }

    protected async Task<HttpResponseMessageValue<T?>?> BotSendAction<T>(FormDataBody body, BotAction action)
    {
        if (!BotRequestUrl.TryGetUri(action, out Uri? uri))
        {
            Log.Error($"Bot发送{action.ToString()}时未找到URI!");
            return default;
        }

        return await HttpRequest.Post<T>(uri!, body, new Dictionary<string, string>
        {
            { "token", Token }
        });
    }

    public void SendMessage(MessageBase message)
    {
        if (!IsRunning)
        {
            Log.Warn("Bot 暂未启动,请启动后在使用此功能!");
            return;
        }

        try
        {
            HttpResponseMessageValue<BotActionResult<SendMessageResult>?>? result =
                BotSendAction<BotActionResult<SendMessageResult>>(message, BotAction.SendMessage)
                    .Result;
            if (result?.Value is null)
            {
                Log.Error("发送信息后返回的数据为空!");
                return;
            }

            if (!result.Response.IsSuccessStatusCode)
            {
                Log.Error($"发送信息失败,返回信息:{result.Value.Message}!状态:{result.Value.Status}");
            }
            else
            {
                Log.Debug($"发送信息返回,信息:\"{result.Value.Message}\",状态:{result.Value.Status}.");
            }
        }
        catch (Exception exception)
        {
            Log.Error("发送信息时出错:" + exception);
        }
    }

    public void Upload(FileInfo file, string fileName = "")
    {
        Upload(file.FullName, fileName);
    }

    public Uri? Upload(string filePath, string fileName = "")
    {
        try
        {
            HttpResponseMessageValue<BotActionResult<UploadResult>?>? result;
            if (!string.IsNullOrEmpty(fileName))
            {
                result = BotSendAction<BotActionResult<UploadResult>>(
                    new FormDataBody(new FormDataBodyFile("file", filePath, fileName)),
                    BotAction.Upload).Result;
            }
            else
            {
                result = BotSendAction<BotActionResult<UploadResult>>(
                    new FormDataBody(new FormDataBodyFile("file", filePath)),
                    BotAction.Upload).Result;
            }

            if (result is null)
            {
                Log.Error("上传文件文件返回为空!");
                return null;
            }

            if (result.Value is null)
            {
                Log.Error("上传文件后返回的数据为空!");
                return null;
            }

            if (!result.Response.IsSuccessStatusCode)
            {
                Log.Error($"上传文件失败,返回信息:{result.Value.Message}!状态:{result.Value.Status}");
                return null;
            }
            else
            {
                Log.Debug(
                    $"上传文件返回,信息:\"{result.Value.Message}\",状态:{result.Value.Status},链接:{result.Value.Result.Uri}");
            }

            return result.Value.Result.Uri;
        }
        catch (Exception exception)
        {
            Log.Error("上传媒体文件遇到错误:" + exception);
            return null;
        }
    }
}