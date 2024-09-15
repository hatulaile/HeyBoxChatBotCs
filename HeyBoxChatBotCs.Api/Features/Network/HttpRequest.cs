using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using HeyBoxChatBotCs.Api.Misc;

namespace HeyBoxChatBotCs.Api.Features.Network;

public delegate void SendingNetworkRequest();

public class HttpRequest
{
    public static JsonSerializerOptions HttpRequestJsonSerializerOptions { get; } = JsonSerializerOptions.Default;
    public static event SendingNetworkRequest? OnSendingNetworkRequest;

    public static async Task<object> Get(Uri uri, Type type, IReadOnlyDictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null
    )
    {
        return JsonSerializer.Deserialize(await Get(uri, headers, path, query), type);
    }

    public static async Task<T> Get<T>(Uri uri, IReadOnlyDictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null)
    {
        return JsonSerializer.Deserialize<T>(await Get(uri, headers, path, query));
    }

    public static async Task<string> Get(Uri uri, IReadOnlyDictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null)
    {
        return await GetResponseMessageAsync(uri.ToString(), headers, path, query).Result.Content.ReadAsStringAsync();
    }

    public static async Task<object> Get(string uri, Type type, IReadOnlyDictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null
    )
    {
        return JsonSerializer.Deserialize(await Get(uri, headers, path, query), type);
    }

    public static async Task<T> Get<T>(string uri, IReadOnlyDictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null)
    {
        return JsonSerializer.Deserialize<T>(await Get(uri, headers, path, query));
    }

    public static async Task<string> Get(string uri, IReadOnlyDictionary<string, string>? headers = null,
        string? path = null, NameValueCollection? query = null)
    {
        return await GetResponseMessageAsync(uri, headers, path, query).Result.Content.ReadAsStringAsync();
    }

    internal static async Task<HttpResponseMessage> GetResponseMessageAsync(string uri,
        IReadOnlyDictionary<string, string>? headers, string? path, NameValueCollection? query)
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


    public static async Task<object> Post(Uri uri, IReadOnlyDictionary<object, object> body, Type type,
        IReadOnlyDictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return JsonSerializer.Deserialize(await Post(uri, body, headers, path, query, contentType, encoding), type);
    }

    public static async Task<T> Post<T>(Uri uri, IReadOnlyDictionary<object, object> body,
        IReadOnlyDictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return JsonSerializer.Deserialize<T>(await Post(uri, body, headers, path, query, contentType, encoding));
    }

    public static async Task<string> Post(Uri uri, IReadOnlyDictionary<object, object> body,
        IReadOnlyDictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return await PostResponseMessageAsync(uri.ToString(), body, headers, path, query, contentType, encoding).Result
            .Content
            .ReadAsStringAsync();
    }

    public static async Task<object> Post(string uri, IReadOnlyDictionary<object, object> body, Type type,
        IReadOnlyDictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return JsonSerializer.Deserialize(await Post(uri, body, headers, path, query, contentType, encoding), type);
    }

    public static async Task<T> Post<T>(string uri, IReadOnlyDictionary<object, object> body,
        IReadOnlyDictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return JsonSerializer.Deserialize<T>(await Post(uri, body, headers, path, query, contentType, encoding));
    }

    public static async Task<string> Post(string uri, IReadOnlyDictionary<object, object> body,
        IReadOnlyDictionary<string, string>? headers = null, string? path = null, NameValueCollection? query = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return await PostResponseMessageAsync(uri, body, headers, path, query, contentType, encoding).Result.Content
            .ReadAsStringAsync();
    }


    internal static async Task<HttpResponseMessage> PostResponseMessageAsync(string uri,
        IReadOnlyDictionary<object, object> body, IReadOnlyDictionary<string, string>? headers, string? path,
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