using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBoxBotCs.Api.Features.Network;

public delegate void SendingNetworkRequest();

public class HttpRequest
{
    public static event SendingNetworkRequest? OnSendingNetworkRequest;

    public static async Task<T> Get<T>(Uri uri, IReadOnlyDictionary<string, IEnumerable<string>>? headers = null)
    {
        return JsonSerializer.Deserialize<T>(await Get(uri, headers));
    }

    public static async Task<string> Get(Uri uri, IReadOnlyDictionary<string, IEnumerable<string>>? headers = null)
    {
        return await GetResponseMessageAsync(uri, headers).Result.Content.ReadAsStringAsync();
    }

    private static async Task<HttpResponseMessage> GetResponseMessageAsync(Uri uri,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers = null)
    {
        using HttpClient httpClient = new HttpClient();
        if (headers is not null)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        OnSendingNetworkRequest?.Invoke();
        return await httpClient.GetAsync(uri);
    }

    public static async Task<T> Post<T>(Uri uri, IReadOnlyDictionary<object, object> body,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers = null,
        string contentType = "application/json;")
    {
        return JsonSerializer.Deserialize<T>(await Post(uri, body, headers, contentType));
    }

    public static async Task<string> Post(Uri uri, IReadOnlyDictionary<object, object> body,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers = null,
        string contentType = "application/json;")
    {
        return await PostResponseMessageAsync(uri, body, headers, contentType).Result.Content.ReadAsStringAsync();
    }


    private static async Task<HttpResponseMessage> PostResponseMessageAsync(Uri uri,
        IReadOnlyDictionary<object, object> body, IReadOnlyDictionary<string, IEnumerable<string>>? headers = null,
        string contentType = "application/json;")
    {
        using HttpClient httpClient = new HttpClient();
        if (headers is not null)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        OnSendingNetworkRequest?.Invoke();
        StringContent stringContent =
            new StringContent(JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, contentType);
        return await httpClient.PostAsync(uri, stringContent);
    }
}