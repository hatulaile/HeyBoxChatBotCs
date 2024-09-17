using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using HeyBoxChatBotCs.Api.Misc;

namespace HeyBoxChatBotCs.Api.Features.Network;

public delegate void SendingNetworkRequest();

public static class HttpRequest
{
    public static JsonSerializerOptions HttpRequestJsonSerializerOptions { get; } = JsonSerializerOptions.Default;
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

    internal static async Task<HttpResponseMessage> GetResponseMessageAsync(string uri,
        Dictionary<string, string>? headers, string? path, NameValueCollection? query)
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
        return await httpClient.GetAsync(HttpMisc.ConstructUrl(uri, path, query));
    }


    public static async Task<HttpResponseMessageValue> Post(Uri uri, string body, Type type,
        Dictionary<string, string>? headers = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        HttpResponseMessageValue<string?> ret = await Post(uri, body, headers, contentType, encoding);
        return new HttpResponseMessageValue(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize(ret.Value, type));
    }

    public static async Task<HttpResponseMessageValue<T?>> Post<T>(Uri uri, string body,
        Dictionary<string, string>? headers = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        HttpResponseMessageValue<string?> ret = await Post(uri, body, headers, contentType, encoding);
        return new HttpResponseMessageValue<T?>(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize<T>(ret.Value));
    }

    public static async Task<HttpResponseMessageValue<string?>> Post(Uri uri, string body,
        Dictionary<string, string>? headers = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        HttpResponseMessage ret = await PostResponseMessageAsync(uri, body, headers, contentType, encoding);
        return new HttpResponseMessageValue<string?>(ret, await ret.Content.ReadAsStringAsync());
    }

    public static async Task<HttpResponseMessageValue> Post(string uri, Dictionary<object, object> body, Type type,
        Dictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        HttpResponseMessageValue<string?> ret = await Post(uri, body, headers, path, query, contentType, encoding);
        return new HttpResponseMessageValue(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize(ret.Value, type));
    }

    public static async Task<HttpResponseMessageValue<T?>> Post<T>(string uri, Dictionary<object, object> body,
        Dictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        HttpResponseMessageValue<string?> ret = await Post(uri, body, headers, path, query, contentType, encoding);
        return new HttpResponseMessageValue<T?>(ret.Response,
            ret.Value is null ? default : JsonSerializer.Deserialize<T>(ret.Value));
    }

    public static async Task<HttpResponseMessageValue<string?>> Post(string uri, Dictionary<object, object> body,
        Dictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        var ret = await PostResponseMessageAsync(uri, body, headers, path, query, contentType, encoding);
        return new HttpResponseMessageValue<string?>(ret, await ret.Content.ReadAsStringAsync());
    }

    internal static async Task<HttpResponseMessage> PostResponseMessageAsync(Uri uri,
        string body, Dictionary<string, string>? headers,
        string contentType, Encoding? encoding)
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
        StringContent stringContent =
            new StringContent(body,
                encoding ?? Encoding.UTF8, contentType);
        return await httpClient.PostAsync(uri, stringContent);
    }

    internal static async Task<HttpResponseMessage> PostResponseMessageAsync(string uri,
        Dictionary<object, object> body, Dictionary<string, string>? headers, string? path,
        NameValueCollection? query,
        string contentType, Encoding? encoding)
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
        StringContent stringContent =
            new StringContent(JsonSerializer.Serialize(body, HttpRequestJsonSerializerOptions),
                encoding ?? Encoding.UTF8, contentType);
        return await httpClient.PostAsync(HttpMisc.ConstructUrl(uri, path, query), stringContent);
    }
}

public class HttpResponseMessageValue
{
    public HttpResponseMessageValue(HttpResponseMessage response, object? value)
    {
        Response = response;
        Value = value;
    }

    public HttpResponseMessage Response { get; init; }
    public object? Value { get; init; }
}

public class HttpResponseMessageValue<TValue>
{
    public HttpResponseMessageValue(HttpResponseMessage response, TValue value)
    {
        Response = response;
        Value = value;
    }

    public HttpResponseMessage Response { get; init; }
    public TValue Value { get; init; }
}