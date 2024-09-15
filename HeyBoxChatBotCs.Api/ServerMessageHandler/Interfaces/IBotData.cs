using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;

public interface IBotData : IData
{
    [JsonPropertyName("bot_id")] public long BotId { get; init; }
}