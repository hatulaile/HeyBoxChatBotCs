using System.Text;

namespace HeyBoxChatBotCs.Api.Features.Network.HttpBody;

public class RawBody : IHttpBody
{
    public RawBody(string json, string contentType = "application/json", Encoding? encoding = null)
    {
        Json = json;
        ContentType = contentType;
        Encoding = encoding ?? Encoding.UTF8;
    }

    public string Json { get; init; }
    public string ContentType { get; init; }
    public Encoding Encoding { get; init; }

    public HttpContent GetContent()
    {
        return new StringContent(Json, Encoding, ContentType);
    }
}