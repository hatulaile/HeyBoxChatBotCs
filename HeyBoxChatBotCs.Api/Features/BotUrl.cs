using System.Collections.Specialized;
using System.Web;
using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Misc;

namespace HeyBoxChatBotCs.Api.Features;

public static class BotRequestUrl
{
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

    private static readonly Dictionary<BotAction, RequestUri> BotActionToUri = new()
    {
        [BotAction.SendMessage] = new RequestUri("https://chat.xiaoheihe.cn/", "/chatroom/v2/channel_msg/send",
            (NameValueCollection?)null),
    };

    public static IReadOnlyDictionary<BotAction, RequestUri> UriDictionary => BotActionToUri;

    public static bool TryGetUri(BotAction action, out Uri? uri)
    {
        uri = null;
        if (!BotActionToUri.TryGetValue(action, out RequestUri? requestUri))
        {
            Log.Error($"获取BOT动作地址失败:{action.ToString()}");
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

public static class BotUrlBase
{
    public const string WEBSOCKET_URL_BASE = "wss://chat.xiaoheihe.cn/";
}