using System.Text;
using System.Text.Json;

namespace HeyBoxChatBotCs.Api.Features.Network;

public delegate void SendingNetworkRequest();

public class HttpRequest
{
    public static JsonSerializerOptions HttpRequestJsonSerializerOptions { get; } = new()
    {
    };

    public static event SendingNetworkRequest? OnSendingNetworkRequest;

    public static async Task<object> Get(Uri uri, Type type,
        IReadOnlyDictionary<string, string>? headers = null
    )
    {
        return JsonSerializer.Deserialize(await Get(uri, headers), type);
    }

    public static async Task<T> Get<T>(Uri uri, IReadOnlyDictionary<string, string>? headers = null)
    {
        return JsonSerializer.Deserialize<T>(await Get(uri, headers));
    }

    public static async Task<string> Get(Uri uri, IReadOnlyDictionary<string, string>? headers = null)
    {
        return await GetResponseMessageAsync(uri, headers).Result.Content.ReadAsStringAsync();
    }

    private static async Task<HttpResponseMessage> GetResponseMessageAsync(Uri uri,
        IReadOnlyDictionary<string, string>? headers)
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

    public static async Task<object> Post(Uri uri, IReadOnlyDictionary<object, object> body, Type type,
        IReadOnlyDictionary<string, string>? headers = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return JsonSerializer.Deserialize(await Post(uri, body, headers, contentType, encoding), type);
    }

    public static async Task<T> Post<T>(Uri uri, IReadOnlyDictionary<object, object> body,
        IReadOnlyDictionary<string, string>? headers = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return JsonSerializer.Deserialize<T>(await Post(uri, body, headers, contentType, encoding));
    }

    public static async Task<string> Post(Uri uri, IReadOnlyDictionary<object, object> body,
        IReadOnlyDictionary<string, string>? headers = null,
        string contentType = "application/json", Encoding? encoding = null)
    {
        return await PostResponseMessageAsync(uri, body, headers, contentType, encoding).Result.Content
            .ReadAsStringAsync();
    }


    private static async Task<HttpResponseMessage> PostResponseMessageAsync(Uri uri,
        IReadOnlyDictionary<object, object> body, IReadOnlyDictionary<string, string>? headers,
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
        return await httpClient.PostAsync(uri, stringContent);
    }
}