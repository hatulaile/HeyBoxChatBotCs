using System.Collections.Frozen;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Misc;

namespace HeyBoxChatBotCs.Api.Features;

public static class BotRequestUrl
{
    private static RequestUri CreateRequestUri(string baseUrl, string? path, NameValueCollection? query = null)
    {
        query ??= [];
        query.Add("chat_os_type", Bot.CHAT_OS_TYPE);
        query.Add("client_type", Bot.CLIENT_TYPE);
        query.Add("chat_version", Bot.CHAT_VERSION);
        return new RequestUri(baseUrl, path, query);
    }

    public static FrozenDictionary<BotOperation, RequestUri> BotActionToUri { get; } =
        new Dictionary<BotOperation, RequestUri>
        {
            {
                BotOperation.Connect,
                CreateRequestUri(Bot.WS_CONNECT_URI, "/chatroom/ws/connect")
            },
            {
                BotOperation.SendMessage,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/channel_msg/send")
            },
            {
                BotOperation.UpdateMessage,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/channel_msg/update")
            },
            {
                BotOperation.DeleteMessage,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/channel_msg/delete")
            },
            {
                BotOperation.Upload,
                CreateRequestUri(Bot.UPLOAD_URI, "/upload")
            },
            {
                BotOperation.GetRoomEmoji,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v3/msg/meme/room/list")
            },
            {
                BotOperation.GetRoomRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/roles")
            },
            {
                BotOperation.EditEmojiName,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/msg/meme/room/edit")
            },
            {
                BotOperation.DeleteEmoji,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/msg/meme/room/del")
            },
            {
                BotOperation.CreateRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/create")
            },
            {
                BotOperation.UpdateRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/update")
            },
            {
                BotOperation.DeleteRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/delete")
            },
            {
                BotOperation.GiveUserRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/grant")
            },
            {
                BotOperation.RevokeUserRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/revoke")
            },
        }.ToFrozenDictionary();


    public static bool TryGetUri(BotOperation operation, [NotNullWhen(true)] out Uri? uri,
        NameValueCollection? extraQuery = null)
    {
        uri = null;
        if (!BotActionToUri.TryGetValue(operation, out RequestUri? requestUri))
        {
            return false;
        }

        uri = HttpMisc.ConstructUrl(requestUri, extraQuery);
        return true;
    }

    public static Uri GetUri(BotOperation operation, NameValueCollection? extraQuery = null)
    {
        if (BotActionToUri.TryGetValue(operation, out RequestUri? requestUri))
        {
            return HttpMisc.ConstructUrl(requestUri, extraQuery);
        }

        throw new Exception($"获取BOT动作地址失败:{operation.ToString()}");
    }
}

public sealed class RequestUri
{
    public RequestUri(string baseUrl, string? path, NameValueCollection? query = null) : this(baseUrl,
        path?.Split(HttpMisc.PathSeparator, StringSplitOptions.RemoveEmptyEntries),
        query)
    {
    }

    public RequestUri(string baseUrl, string[]? path, NameValueCollection? query)
    {
        BaseUrl = baseUrl;
        Path = path;
        Query = query;
    }

    public string BaseUrl { get; }
    public string[]? Path { get; }
    public NameValueCollection? Query { get; }
}