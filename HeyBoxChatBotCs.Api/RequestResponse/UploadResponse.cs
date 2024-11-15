using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.RequestResponse;

internal class UploadResponse
{
    [JsonPropertyName("url")] public required Uri Uri { get; init; }
}