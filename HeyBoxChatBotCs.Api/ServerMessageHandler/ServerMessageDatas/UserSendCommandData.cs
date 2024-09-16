using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.EventArgs.Interfaces;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.ServerMessageHandler.Interfaces;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageDatas;

public class UserSendCommandData : IServerMessageData, IBotData, IChannelData, ISendMessageData, IRoomData, IUserData,
    ICommandData
{
    [JsonPropertyName("bot_id")] public long BotId { get; init; }
    [JsonPropertyName("msg_id")] public string MessageId { get; init; }
    [JsonPropertyName("send_time")] public long SendTime { get; init; }

    [JsonPropertyName("channel_base_info")]
    public Channel Channel { get; init; }

    [JsonPropertyName("room_base_info")] public Room Room { get; init; }
    [JsonPropertyName("sender_info")] public User User { get; init; }


    [JsonPropertyName("command_info")] public CommandInfo Command { get; init; }

    public void InvokeRelatedEvent()
    {
        Features.Events.User.OnUserSendCommand(this);
        Commands.CommandSystem.UserCommandProcessor.InvokeUserSendCommandAction(this);
    }
}