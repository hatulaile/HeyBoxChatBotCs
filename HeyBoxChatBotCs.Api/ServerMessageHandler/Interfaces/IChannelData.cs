﻿using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;

public interface IChannelData : IData
{
    [JsonPropertyName("channel_base_info")]
    public Channel Channel { get; init; }
}