using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.Bot.BotActionResult;

public class UploadResult
{
    [JsonPropertyName("url")]
    public required Uri Uri { get; init; }
}