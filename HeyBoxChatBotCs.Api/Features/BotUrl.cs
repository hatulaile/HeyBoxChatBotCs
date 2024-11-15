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

    public static FrozenDictionary<BotOperations, RequestUri> BotActionToUri { get; } =
        new Dictionary<BotOperations, RequestUri>
        {
            {
                BotOperations.Connect,
                CreateRequestUri(Bot.WS_CONNECT_URI, "/chatroom/ws/connect")
            },
            {
                BotOperations.SendMessage,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/channel_msg/send")
            },
            {
                BotOperations.UpdateMessage,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/channel_msg/update")
            },
            {
                BotOperations.DeleteMessage,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/channel_msg/delete")
            },
            {
                BotOperations.ReplyMessageEmoji,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/channel_msg/emoji/reply")
            },
            {
                BotOperations.Upload,
                CreateRequestUri(Bot.UPLOAD_URI, "/upload")
            },
            {
                BotOperations.GetRoomEmote,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v3/msg/meme/room/list")
            },
            {
                BotOperations.GetRoomRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/roles")
            },
            {
                BotOperations.EditEmoteName,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/msg/meme/room/edit")
            },
            {
                BotOperations.DeleteEmote,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/msg/meme/room/del")
            },
            {
                BotOperations.CreateRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/create")
            },
            {
                BotOperations.UpdateRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/update")
            },
            {
                BotOperations.DeleteRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/delete")
            },
            {
                BotOperations.GiveUserRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/grant")
            },
            {
                BotOperations.RevokeUserRole,
                CreateRequestUri(Bot.REQUEST_URI, "/chatroom/v2/room_role/revoke")
            },
        }.ToFrozenDictionary();


    public static bool TryGetUri(BotOperations operations, [NotNullWhen(true)] out Uri? uri,
        NameValueCollection? extraQuery = null)
    {
        uri = null;
        if (!BotActionToUri.TryGetValue(operations, out RequestUri? requestUri))
        {
            return false;
        }

        uri = HttpMisc.ConstructUrl(requestUri, extraQuery);
        return true;
    }

    public static Uri GetUri(BotOperations operations, NameValueCollection? extraQuery = null)
    {
        if (BotActionToUri.TryGetValue(operations, out RequestUri? requestUri))
        {
            return HttpMisc.ConstructUrl(requestUri, extraQuery);
        }

        throw new Exception($"获取BOT动作地址失败:{operations.ToString()}");
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