using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.Misc;

public static partial class HttpMisc
{
    internal static readonly char[] PathSeparator = ['/', '\\'];

    public static Uri ConstructUrl(BotRequestUrl.RequestUri requestUrl)
    {
        return ConstructUrl(requestUrl.BaseUrl, requestUrl.Path, requestUrl.Query);
    }

    public static Uri ConstructUrl(string uri, string? path = null, string? query = null)
    {
        return ConstructUrl(uri, path, query is not null ? HttpUtility.ParseQueryString(query) : null);
    }


    public static Uri ConstructUrl(string uri, string? paths = null, NameValueCollection? query = null)
    {
        return ConstructUrl(uri, paths?.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries),
            query);
    }

    public static Uri ConstructUrl(string uri, string[]? paths = null, NameValueCollection? query = null)
    {
        ArgumentNullException.ThrowIfNull(uri);
        uri = uri.Trim(' ', '/', '\\');
        if (!ValidHttpUrl(uri, out _))
        {
            throw new UriFormatException("传入的链接不是一个可用的链接");
        }

        StringBuilder sb = new StringBuilder(uri);
        if (paths is not null)
        {
            foreach (string path in paths)
            {
                sb.Append($"/{HttpUtility.UrlEncode(path)}");
            }
        }

        if (query is not null)
        {
            sb.Append('?');
            foreach (string? key in query)
            {
                if (key is null)
                {
                    continue;
                }

                string[]? values = query.GetValues(key);
                if (values is null)
                {
                    sb.Append($"{HttpUtility.UrlEncode(key)}=&");
                    continue;
                }

                foreach (string value in values)
                {
                    sb.Append($"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}&");
                }
            }

            if (sb[^1] is '&')
            {
                sb.Remove(sb.Length - 1, 1);
            }
        }

        Log.Debug($"最后编码URL:{sb}");
        return new Uri(sb.ToString());
    }

    //此处验证来源于CSDN博客 https://blog.csdn.net/sD7O95O/article/details/120897375
    public static bool ValidHttpUrl(string s, out Uri? resultUri)
    {
        if (!MyRegex().IsMatch(s))
        {
            s = "http://" + s;
        }

        if (Uri.TryCreate(s, UriKind.Absolute, out resultUri))
            return resultUri.Scheme == Uri.UriSchemeHttp ||
                   resultUri.Scheme == Uri.UriSchemeHttps ||
                   resultUri.Scheme == Uri.UriSchemeWs ||
                   resultUri.Scheme == Uri.UriSchemeWss;

        return false;
    }

    [GeneratedRegex(@"^https?:\/\/", RegexOptions.IgnoreCase, "zh-CN")]
    private static partial Regex MyRegex();
}