using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features.BotActionResult;
using HeyBoxChatBotCs.Api.Features.BotRequestDto;
using HeyBoxChatBotCs.Api.Features.BotRequestDto.Message;
using HeyBoxChatBotCs.Api.Features.Network;
using HeyBoxChatBotCs.Api.Features.Network.HttpBody;

namespace HeyBoxChatBotCs.Api.Features;

public class Bot
{
    public const string REQUEST_URI = "https://chat.xiaoheihe.cn/";
    public const string WS_CONNECT_URI = "wss://chat.xiaoheihe.cn/";
    public const string UPLOAD_URI = "https://chat-upload.xiaoheihe.cn/";
    public const string CHAT_OS_TYPE = "bot";
    public const string CLIENT_TYPE = "heybox_chat";
    public const string CHAT_VERSION = "1.29.0";

    public static readonly JsonSerializerOptions BotOperationJsonSerializerOptions = new()
    {
        AllowTrailingCommas = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public Bot(string id, string token)
    {
        Instance?.CloseAsync();
        Id = id;
        Token = token;
        Instance = this;
    }

    [NotNull] public static Bot? Instance { get; private set; }

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

    protected static void ThrowIfNotOk<T>(HttpResponseMessageValue<BotOperationResult<T>?> res)
    {
        if (res.Value is null)
        {
            throw new Exception("返回的数据为空!");
        }

        if (!res.Response.IsSuccessStatusCode || res.Value.Status is not "ok")
        {
            throw new Exception($"请求失败,原因:{res.Value.Message},状态:{res.Value.Status}!");
        }
    }

    protected async Task<HttpResponseMessageValue<BotOperationResult<T>>> BotGetAsync<T>(BotOperation operation,
        NameValueCollection? extraQuery = null)
    {
        if (BotCts is null || BotCts.IsCancellationRequested)
        {
            throw new Exception("Bot 暂未启动");
        }

        if (!BotRequestUrl.TryGetUri(operation, out Uri? uri, extraQuery))
        {
            throw new Exception($"Bot发送{operation.ToString()}时未找到URI!");
        }

        HttpResponseMessageValue<BotOperationResult<T>?> res = await HttpRequest.Get<BotOperationResult<T>>(uri,
            new Dictionary<string, string>
            {
                { "token", Token }
            });
        ThrowIfNotOk(res);
        return res!;
    }

    protected async Task<HttpResponseMessageValue<BotOperationResult<T>>> BotSendRequestAsync<T>(object body,
        BotOperation operation,
        string contentType = "application/json")
    {
        if (BotCts is null || BotCts.IsCancellationRequested)
        {
            throw new Exception("Bot 暂未启动");
        }

        if (!BotRequestUrl.TryGetUri(operation, out Uri? uri))
        {
            throw new Exception($"Bot发送{operation.ToString()}时未找到URI!");
        }

        string json = body as string ?? JsonSerializer.Serialize(body, BotOperationJsonSerializerOptions);
        Log.Debug("BOT动作的JSON为:" + json);
        HttpResponseMessageValue<BotOperationResult<T>?> res = await HttpRequest.Post<BotOperationResult<T>>(uri,
            new RawBody(json, contentType),
            new Dictionary<string, string>
            {
                { "token", Token }
            });
        ThrowIfNotOk(res);
        return res!;
    }

    protected async Task<HttpResponseMessageValue<BotOperationResult<T>>> BotSendRequestAsync<T>(FormDataBody body,
        BotOperation operation)
    {
        if (BotCts is null || BotCts.IsCancellationRequested)
        {
            throw new Exception("Bot 暂未启动");
        }

        if (!BotRequestUrl.TryGetUri(operation, out Uri? uri))
        {
            throw new Exception($"Bot发送{operation.ToString()}时未找到URI!");
        }

        HttpResponseMessageValue<BotOperationResult<T>?> res = await HttpRequest.Post<BotOperationResult<T>>(uri, body,
            new Dictionary<string, string>
            {
                { "token", Token }
            });
        ThrowIfNotOk(res);
        return res!;
    }

    public async Task<SendMessageResult> SendMessageAsync(MessageBase message)
    {
        var res = await BotSendRequestAsync<SendMessageResult>(message, BotOperation.SendMessage);
        Log.Debug($"发送信息返回:{res.Value},{res.Value.Result}");
        return res.Value.Result;
    }

    public async Task<UpdateMarkdownMessageResult> UpdateMarkdownMessageAsync(UpdateMarkdownMessageDto dto)
    {
        HttpResponseMessageValue<BotOperationResult<UpdateMarkdownMessageResult>> res =
            await BotSendRequestAsync<UpdateMarkdownMessageResult>(dto, BotOperation.UpdateMessage);
        Log.Debug($"更新信息角色返回:{res.Value}");
        return res.Value.Result;
    }

    public async Task DeleteMessageAsync(DeleteEmojiDto dto)
    {
        var res = await BotSendRequestAsync<object>(dto, BotOperation.DeleteMessage);
        Log.Debug($"删除信息返回:{res.Value},{res.Value.Result}");
    }


    public async Task<Uri> UploadAsync(string filePath, string fileName = "")
    {
        HttpResponseMessageValue<BotOperationResult<UploadResult>> result;
        if (!string.IsNullOrEmpty(fileName))
        {
            result = await BotSendRequestAsync<UploadResult>(
                new FormDataBody(new FormDataBodyFile("file", filePath, fileName)),
                BotOperation.Upload);
        }
        else
        {
            result = await BotSendRequestAsync<UploadResult>(
                new FormDataBody(new FormDataBodyFile("file", filePath)),
                BotOperation.Upload);
        }

        Log.Debug(
            $"上传文件返回:{result.Value},链接:{result.Value.Result.Uri}");

        return result.Value.Result.Uri;
    }

    public async Task<RoomEmojiResult> GetRoomEmojiAsync(string roomId)
    {
        var res = await BotGetAsync<RoomEmojiResult>(BotOperation.GetRoomEmoji, new NameValueCollection
        {
            { "room_id", roomId }
        });
        Log.Debug($"获取房间表情返回:{res.Value}");
        return res.Value.Result;
    }

    public async Task<Role[]> GetRoleAsync(string roomId)
    {
        var res = await BotGetAsync<RoomRoleResult>(BotOperation.GetRoomRole, new NameValueCollection
        {
            { "room_id", roomId }
        });
        Log.Debug($"获取房间表情返回:{res.Value}");
        return res.Value.Result.Roles;
    }

    public async Task EditEmojiNameAsync(EditEmojiNameDto dto)
    {
        var res = await BotSendRequestAsync<object>(dto, BotOperation.EditEmojiName);
        Log.Debug($"更改表情名称返回:{res.Value}");
    }

    public async Task DeleteEmojiAsync(DeleteEmojiDto dto)
    {
        var res = await BotSendRequestAsync<object>(dto, BotOperation.DeleteEmoji);
        Log.Debug($"删除表情返回:{res.Value}");
    }

    public async Task<Role> CreateRoleAsync(CreateRoleDto dto)
    {
        var res = await BotSendRequestAsync<CreateRoleResult>(dto, BotOperation.CreateRole);
        Log.Debug($"创造角色返回:{res.Value}");
        return res.Value.Result.Role;
    }

    public async Task<Role> UpdateRoleAsync(UpdateRoleDto dto)
    {
        var res = await BotSendRequestAsync<UpdateRoleResult>(dto, BotOperation.UpdateRole);
        Log.Debug($"更新角色返回:{res.Value}");
        return res.Value.Result.Role;
    }

    public async Task<OperationUserRoleResult.RoleUser> GiveRoleAsync(OperationUserRoleDto dto)
    {
        HttpResponseMessageValue<BotOperationResult<OperationUserRoleResult>> res =
            await BotSendRequestAsync<OperationUserRoleResult>(dto, BotOperation.GiveUserRole);
        Log.Debug($"给予用户角色返回:{res.Value}");
        return res.Value.Result.User;
    }

    public async Task<OperationUserRoleResult.RoleUser> RevokeRoleAsync(OperationUserRoleDto dto)
    {
        HttpResponseMessageValue<BotOperationResult<OperationUserRoleResult>> res =
            await BotSendRequestAsync<OperationUserRoleResult>(dto, BotOperation.RevokeUserRole);
        Log.Debug($"剥夺用户角色返回:{res.Value}");
        return res.Value.Result.User;
    }

    public async Task DeleteRoleAsync(DeleteRoomRoleDto dto)
    {
        HttpResponseMessageValue<BotOperationResult<object>> res =
            await BotSendRequestAsync<object>(dto, BotOperation.DeleteRole);
        Log.Debug($"删除用户角色返回:{res.Value}");
    }
}