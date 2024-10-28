using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Features.BotActionResult;

public class BotOperationResult<TResult>
{
    [JsonPropertyName("msg")] public required string Message { get; init; }
    [JsonPropertyName("result")] public required TResult Result { get; init; }
    [JsonPropertyName("status")] public required string Status { get; init; }

    public override string ToString()
    {
        return $"Message:{Message},Status:{Status}";
    }
}