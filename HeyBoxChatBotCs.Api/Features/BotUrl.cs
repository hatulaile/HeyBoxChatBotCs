using System.Collections.Specialized;
using System.Web;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Misc;

namespace HeyBoxChatBotCs.Api.Features;

public static class BotRequestUrl
{
    private static Dictionary<BotAction, RequestUri> BotActionToUri { get; } = new()
    {
        {
            BotAction.Connect, new RequestUri("wss://chat.xiaoheihe.cn/", "/chatroom/ws/connect",
                new NameValueCollection
                {
                    { "chat_os_type", "bot" },
                    { "client_type", "heybox_chat" },
                    { "chat_version", "1.27.2" }
                })
        },
        {
            BotAction.SendMessage, new RequestUri("https://chat.xiaoheihe.cn/", "/chatroom/v2/channel_msg/send",
                new NameValueCollection
                {
                    { "chat_os_type", "bot" },
                    { "client_type", "heybox_chat" },
                    { "chat_version", "1.27.2" }
                })
        },
        {
            BotAction.Upload,
            new RequestUri("https://chat-upload.xiaoheihe.cn/", "/upload", new NameValueCollection()
            {
                { "chat_os_type", "bot" },
                { "client_type", "heybox_chat" },
                { "chat_version", "1.27.2" }
            })
        }
    };

    public static IReadOnlyDictionary<BotAction, RequestUri> UriDictionary => BotActionToUri;

    public static bool TryGetUri(BotAction action, out Uri? uri)
    {
        uri = null;
        if (!BotActionToUri.TryGetValue(action, out RequestUri? requestUri))
        {
            return false;
        }

        uri = HttpMisc.ConstructUrl(requestUri);
        return true;
    }

    public static Uri? GetUri(BotAction action)
    {
        if (BotActionToUri.TryGetValue(action, out RequestUri? requestUri))
        {
            return HttpMisc.ConstructUrl(requestUri);
        }

        Log.Error($"获取BOT动作地址失败:{action.ToString()}");
        return null;
    }
}

public sealed class RequestUri
{
    public RequestUri(string baseUrl, string? path, string? query) : this(baseUrl,
        path?.Split(HttpMisc.PathSeparator, StringSplitOptions.RemoveEmptyEntries),
        query is not null ? HttpUtility.ParseQueryString(query) : null)
    {
    }

    public RequestUri(string baseUrl, string? path, NameValueCollection? query) : this(baseUrl,
        path?.Split(HttpMisc.PathSeparator, StringSplitOptions.RemoveEmptyEntries), query)
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