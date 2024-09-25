using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;

public interface IBotData : IData
{
    [JsonPropertyName("bot_id")] long BotId { get; init; }
}