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

    public Bot(string id, string token)
    {
        Instance?.CloseAsync();
        Id = id;
        Token = token;
        Instance = this;
    }

    public static Bot? Instance { get; private set; }

    public string Id { get; }
    public string Token { get; }

    public BotWebSocket? BotWebSocket { get; private set; }

    private CancellationTokenSource? BotCts { get; set; }

    public static event EventHandler? BotStart;

    public static event EventHandler? BotClose;

    /// <summary>
    ///     开启Bot 此方法会阻塞程序!
    /// </summary>
    public async Task StartAsync()
    {
        if (BotCts is not null or { IsCancellationRequested: false })
        {
            Log.Error("请不要重复启动机器人!");
            return;
        }

        BotWebSocket ??= new BotWebSocket(this);
        await BotWebSocket.Start();
        await new Loader.Loader().Run();
        BotCts = new CancellationTokenSource();
        BotStart?.Invoke(this);
        Log.Info("控制台命令已启用,可以输入!");
        _ = ConsoleCommandProcessor.Run();
        try
        {
            await Task.Delay(Timeout.Infinite, BotCts.Token);
        }
        catch (ObjectDisposedException)
        {
            Log.Debug($"{Id}已被终止!");
        }
        catch (TaskCanceledException)
        {
            Log.Debug($"{Id}已被终止!");
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }

    public Task CloseAsync()
    {
        if (BotCts is null || BotWebSocket is null || BotCts.IsCancellationRequested)
        {
            Log.Warn("你还未启用Bot,无法关闭!");
        }

        Log.Info($"{Id} 正在关闭!");
        ConsoleCommandProcessor.ConsoleReadCts?.Cancel();
        BotCts?.Cancel();
        BotCts?.Dispose();
        BotClose?.Invoke(this);
        BotWebSocket?.Dispose();
        return Task.CompletedTask;
    }

    protected async Task<HttpResponseMessageValue<T?>?> BotSendActionAsync<T>(object body, BotAction action,
        string contentType = "application/json")
    {
        if (!BotRequestUrl.TryGetUri(action, out Uri? uri))
        {
            Log.Error($"Bot发送{action.ToString()}时未找到URI!");
            return default;
        }

        string json = JsonSerializer.Serialize(body, BotActionJsonSerializerOptions);
        Log.Debug("BOT动作的JSON为:" + json);
        return await HttpRequest.Post<T>(uri, new RawBody(json, contentType), new Dictionary<string, string>
        {
            { "token", Token }
        });
    }

    protected async Task<HttpResponseMessageValue<T?>?> BotSendActionAsync<T>(FormDataBody body, BotAction action)
    {
        if (!BotRequestUrl.TryGetUri(action, out Uri? uri))
        {
            Log.Error($"Bot发送{action.ToString()}时未找到URI!");
            return default;
        }

        return await HttpRequest.Post<T>(uri, body, new Dictionary<string, string>
        {
            { "token", Token }
        });
    }

    public async Task SendMessageAsync(MessageBase message)
    {
        if (BotCts is null || BotCts.IsCancellationRequested)
        {
            Log.Warn("Bot 暂未启动,请启动后在使用此功能!");
            return;
        }

        try
        {
            HttpResponseMessageValue<BotActionResult<SendMessageResult>?>? result =
                await BotSendActionAsync<BotActionResult<SendMessageResult>>(message, BotAction.SendMessage);
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

    public async Task<Uri?> UploadAsync(FileInfo file, string fileName = "")
    {
        return await UploadAsync(file.FullName, fileName);
    }

    public async Task<Uri?> UploadAsync(string filePath, string fileName = "")
    {
        if (BotCts is null || BotCts.IsCancellationRequested)
        {
            Log.Warn("Bot 暂未启动,请启动后在使用此功能!");
            return null;
        }

        try
        {
            HttpResponseMessageValue<BotActionResult<UploadResult>?>? result;
            if (!string.IsNullOrEmpty(fileName))
            {
                result = await BotSendActionAsync<BotActionResult<UploadResult>>(
                    new FormDataBody(new FormDataBodyFile("file", filePath, fileName)),
                    BotAction.Upload);
            }
            else
            {
                result = await BotSendActionAsync<BotActionResult<UploadResult>>(
                    new FormDataBody(new FormDataBodyFile("file", filePath)),
                    BotAction.Upload);
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

            Log.Debug(
                $"上传文件返回,信息:\"{result.Value.Message}\",状态:{result.Value.Status},链接:{result.Value.Result.Uri}");

            return result.Value.Result.Uri;
        }
        catch (Exception exception)
        {
            Log.Error("上传媒体文件遇到错误:" + exception);
            return null;
        }
    }
}