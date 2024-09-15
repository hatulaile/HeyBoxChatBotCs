using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;

public interface IUserData : IData
{
    [JsonPropertyName("sender_info")] public User User { get; init; }
}