using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;

public interface ICommandData : IData
{
    [JsonPropertyName("command_info")] public CommandInfo Command { get; init; }
}