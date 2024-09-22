using System.Collections.Specialized;
using System.Text.Json;
using HeyBoxChatBotCs.Api.Features.Network.HttpBody;
using HeyBoxChatBotCs.Api.Misc;

namespace HeyBoxChatBotCs.Api.Features.Network;

public delegate void SendingNetworkRequest();

public static class HttpRequest
{
    public static JsonSerializerOptions HttpRequestJsonSerializerOptions { get; } = new();

    public static event SendingNetworkRequest? OnSendingNetworkRequest;

    public static async Task<HttpResponseMessageValue> Get(Uri uri, Type type,
        Dictionary<string, string>? headers = null
    )
    {
        HttpResponseMessageValue<string?> ret = await Get(uri, headers);
        return new HttpResponseMessageValue(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize(ret.Value, type));
    }

    public static async Task<HttpResponseMessageValue<T?>> Get<T>(Uri uri, Dictionary<string, string>? headers = null)
    {
        HttpResponseMessageValue<string?> ret = await Get(uri, headers);
        return new HttpResponseMessageValue<T?>(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize<T>(ret.Value));
    }

    public static async Task<HttpResponseMessageValue<string?>> Get(Uri uri, Dictionary<string, string>? headers = null)
    {
        var ret = await GetResponseMessageAsync(uri, headers);
        return new HttpResponseMessageValue<string?>(ret, await ret.Content.ReadAsStringAsync());
    }

    public static async Task<HttpResponseMessageValue> Get(string uri, Type type,
        Dictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null
    )
    {
        HttpResponseMessageValue<string?> ret = await Get(uri, headers, path, query);
        return new HttpResponseMessageValue(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize(ret.Value, type));
    }

    public static async Task<HttpResponseMessageValue<T?>> Get<T>(string uri,
        Dictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null)
    {
        HttpResponseMessageValue<string?> ret = await Get(uri, headers, path, query);
        return new HttpResponseMessageValue<T?>(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize<T>(ret.Value));
    }

    public static async Task<HttpResponseMessageValue<string?>> Get(string uri,
        Dictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null)
    {
        var ret = await GetResponseMessageAsync(uri, headers, path, query);
        return new HttpResponseMessageValue<string?>(ret, await ret.Content.ReadAsStringAsync());
    }

    internal static async Task<HttpResponseMessage> GetResponseMessageAsync(string uri,
        Dictionary<string, string>? headers, string? path, NameValueCollection? query)
    {
        return await GetResponseMessageAsync(HttpMisc.ConstructUrl(uri, path, query), headers);
    }

    internal static async Task<HttpResponseMessage> GetResponseMessageAsync(Uri uri,
        Dictionary<string, string>? headers)
    {
        using HttpClient httpClient = new HttpClient();
        if (headers is not null)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        OnSendingNetworkRequest?.Invoke();
        return await httpClient.GetAsync(uri);
    }


    public static async Task<HttpResponseMessageValue> Post(Uri uri, IHttpBody body, Type type,
        Dictionary<string, string>? headers = null)
    {
        HttpResponseMessageValue<string?> ret = await Post(uri, body, headers);
        return new HttpResponseMessageValue(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize(ret.Value, type));
    }

    public static async Task<HttpResponseMessageValue<T?>> Post<T>(Uri uri, IHttpBody body,
        Dictionary<string, string>? headers = null)
    {
        HttpResponseMessageValue<string?> ret = await Post(uri, body, headers);
        return new HttpResponseMessageValue<T?>(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize<T>(ret.Value));
    }

    public static async Task<HttpResponseMessageValue<string?>> Post(Uri uri, IHttpBody body,
        Dictionary<string, string>? headers = null)
    {
        HttpResponseMessage ret = await PostResponseMessageAsync(uri, body, headers);
        return new HttpResponseMessageValue<string?>(ret, await ret.Content.ReadAsStringAsync());
    }

    public static async Task<HttpResponseMessageValue> Post(string uri, IHttpBody body, Type type,
        Dictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null)
    {
        HttpResponseMessageValue<string?> ret = await Post(uri, body, headers, path, query);
        return new HttpResponseMessageValue(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize(ret.Value, type));
    }

    public static async Task<HttpResponseMessageValue<T?>> Post<T>(string uri, IHttpBody body,
        Dictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null)
    {
        HttpResponseMessageValue<string?> ret = await Post(uri, body, headers, path, query);
        return new HttpResponseMessageValue<T?>(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize<T>(ret.Value));
    }

    public static async Task<HttpResponseMessageValue<string?>> Post(string uri, IHttpBody body,
        Dictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null)
    {
        var ret = await PostResponseMessageAsync(uri, body, headers, path, query);
        return new HttpResponseMessageValue<string?>(ret, await ret.Content.ReadAsStringAsync());
    }

    internal static async Task<HttpResponseMessage> PostResponseMessageAsync(string uri,
        IHttpBody body, Dictionary<string, string>? headers, string? path,
        NameValueCollection? query)
    {
        return await PostResponseMessageAsync(HttpMisc.ConstructUrl(uri, path, query),
            body, headers);
    }

    internal static async Task<HttpResponseMessage> PostResponseMessageAsync(Uri uri,
        IHttpBody body, Dictionary<string, string>? headers)
    {
        using HttpClient httpClient = new HttpClient();
        if (headers is not null)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        using HttpContent content = body.GetContent();
        OnSendingNetworkRequest?.Invoke();
        return await httpClient.PostAsync(uri, content);
    }
}