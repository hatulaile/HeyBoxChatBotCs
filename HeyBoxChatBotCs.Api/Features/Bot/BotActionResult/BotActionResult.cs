using System.Text.Json.Serialization;

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

namespace HeyBoxChatBotCs.Api.Features.Bot.BotActionResult;

public class BotActionResult<TResult> where TResult : new()
{
    [JsonPropertyName("msg")] public string Message { get; init; }
    [JsonPropertyName("result")] public TResult Result { get; init; }
    [JsonPropertyName("status")] public string Status { get; init; }
}