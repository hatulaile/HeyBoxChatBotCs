using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Commands.CommandSystem;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features.Emote;
using HeyBoxChatBotCs.Api.Network;
using HeyBoxChatBotCs.Api.Network.HttpBody;
using HeyBoxChatBotCs.Api.RequestParameters.Emoji;
using HeyBoxChatBotCs.Api.RequestParameters.Message;
using HeyBoxChatBotCs.Api.RequestParameters.Role;
using HeyBoxChatBotCs.Api.RequestResponse;

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

    [AllowNull] public static Bot Instance { get; private set; }

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
        await BotWebSocket.Start().ConfigureAwait(false);
        await new Loader.Loader().Run().ConfigureAwait(false);
        BotCts = new CancellationTokenSource();
        BotStart?.Invoke(this);
        Log.Info("控制台命令已启用,可以输入!");
        _ = ConsoleCommandProcessor.RunAsync();
        try
        {
            await Task.Delay(Timeout.Infinite, BotCts.Token).ConfigureAwait(false);
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

    #region Bot

    protected static void ThrowIfNotOk<T>(HttpResponseMessageValue<BotResponse<T>?> res)
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

    protected async Task<T> BotGetAsync<T>(BotOperations operations,
        NameValueCollection? extraQuery = null)
    {
        if (BotCts is null || BotCts.IsCancellationRequested)
        {
            throw new Exception("Bot 暂未启动");
        }

        if (!BotRequestUrl.TryGetUri(operations, out Uri? uri, extraQuery))
        {
            throw new Exception($"Bot发送{operations.ToString()}时未找到URI!");
        }

        HttpResponseMessageValue<BotResponse<T>?> res = await HttpRequest.GetAsync<BotResponse<T>>(
            uri,
            new Dictionary<string, string>
            {
                { "token", Token }
            }).ConfigureAwait(false);
        ThrowIfNotOk(res);
        return res.Value!.Result;
    }

    protected async Task<T> BotSendAsync<T>(object body,
        BotOperations operations,
        string contentType = "application/json")
    {
        string json = body as string ?? JsonSerializer.Serialize(body, BotOperationJsonSerializerOptions);
        return await BotSendAsync<T>(new RawBody(json, contentType), operations).ConfigureAwait(false);
    }

    protected async Task<T> BotSendAsync<T>(IHttpBody body,
        BotOperations operations)
    {
        if (BotCts is null || BotCts.IsCancellationRequested)
        {
            throw new Exception("Bot 暂未启动");
        }

        if (!BotRequestUrl.TryGetUri(operations, out Uri? uri))
        {
            throw new Exception($"Bot发送{operations.ToString()}时未找到URI!");
        }

        HttpResponseMessageValue<BotResponse<T>?> res = await HttpRequest.PostAsync<BotResponse<T>>(
            uri,
            body,
            new Dictionary<string, string>
            {
                { "token", Token }
            }).ConfigureAwait(false);
        ThrowIfNotOk(res);
        return res.Value!.Result;
    }

    #endregion

    #region File

    public async Task<Uri> UploadAsync(string filePath, string fileName = "")
    {
        UploadResponse result;
        if (!string.IsNullOrEmpty(fileName))
        {
            result = await BotSendAsync<UploadResponse>(
                new FormDataBody(new FormDataBodyFile("file", filePath, fileName)),
                BotOperations.Upload).ConfigureAwait(false);
        }
        else
        {
            result = await BotSendAsync<UploadResponse>(
                new FormDataBody(new FormDataBodyFile("file", filePath)),
                BotOperations.Upload).ConfigureAwait(false);
        }

        return result.Uri;
    }

    #endregion

    #region Message

    public async Task<string> SendMessageAsync(SendMessageParamsBase args)
    {
        var res = await BotSendAsync<SendMessageResponse>(args, BotOperations.SendMessage)
            .ConfigureAwait(false);
        return res.MessageId;
    }

    public async Task UpdateMessageAsync(UpdateMessageParams args)
    {
        await BotSendAsync<UpdateMessageResponse>(args, BotOperations.UpdateMessage)
            .ConfigureAwait(false);
    }

    public async Task DeleteMessageAsync(DeleteEmoteParams args)
    {
        await BotSendAsync<object>(args, BotOperations.DeleteMessage).ConfigureAwait(false);
    }


    public async Task ReplyMessageEmojiAsync(ReplyEmojiParams args)
    {
        await BotSendAsync<object>(args, BotOperations.ReplyMessageEmoji).ConfigureAwait(false);
    }

    #endregion

    #region Emote

    public async Task<IEmote[]> GetRoomEmoteAsync(string roomId)
    {
        var res = await BotGetAsync<RoomEmojiResponse>(BotOperations.GetRoomEmote, new NameValueCollection
        {
            { "room_id", roomId }
        }).ConfigureAwait(false);
        return
        [
            ..res.Emojis.Select(x =>
                new Emoji(x.User, roomId, x.MemeInfo.Extensions, x.MemeInfo.Path, x.MemeInfo.CreateTime)),
            ..res.Stickers.Select(x =>
                new Sticker(x.User, roomId, x.MemeInfo.Extensions, x.MemeInfo.Path, x.MemeInfo.CreateTime)),
        ];
    }


    public async Task ModifyEmoteNameAsync(ModifyEmoteParams args)
    {
        await BotSendAsync<object>(args, BotOperations.EditEmoteName).ConfigureAwait(false);
    }

    public async Task DeleteEmoteAsync(DeleteEmoteParams args)
    {
        await BotSendAsync<object>(args, BotOperations.DeleteEmote).ConfigureAwait(false);
    }

    #endregion

    #region Role

    public async Task<Role[]> GetRoleAsync(string roomId)
    {
        var res = await BotGetAsync<RoomRoleResponse>(BotOperations.GetRoomRole, new NameValueCollection
        {
            { "room_id", roomId }
        }).ConfigureAwait(false);
        return res.Roles;
    }

    public async Task<Role> CreateRoleAsync(CreateRoleParams args)
    {
        var res = await BotSendAsync<CreateRoleResponse>(args, BotOperations.CreateRole).ConfigureAwait(false);
        return res.Role;
    }

    public async Task<Role> ModifyRoleAsync(UpdateRoleParams args)
    {
        var res = await BotSendAsync<ModifyRoleResponse>(args, BotOperations.UpdateRole).ConfigureAwait(false);
        return res.Role;
    }

    public async Task<OperationUserRoleResponse.RoleUser> GiveRoleAsync(ModifyRoleParams args)
    {
        OperationUserRoleResponse res =
            await BotSendAsync<OperationUserRoleResponse>(args, BotOperations.GiveUserRole).ConfigureAwait(false);
        return res.User;
    }

    public async Task<OperationUserRoleResponse.RoleUser> RevokeRoleAsync(ModifyRoleParams args)
    {
        OperationUserRoleResponse res =
            await BotSendAsync<OperationUserRoleResponse>(args, BotOperations.RevokeUserRole)
                .ConfigureAwait(false);
        return res.User;
    }

    public async Task DeleteRoleAsync(DeleteRoleParams args)
    {
        await BotSendAsync<object>(args, BotOperations.DeleteRole).ConfigureAwait(false);
    }

    #endregion
}