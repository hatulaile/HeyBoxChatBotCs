using HeyBoxChatBotCs.Api.EventArgs.Interfaces;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace HeyBoxChatBotCs.Api.EventArgs.User;

public class UserSendCommandEventArgs : ISendCommand, ICommand
{
    public UserSendCommandEventArgs(long botId, Channel channel, Room room, Features.User user, string messageId,
        long sendTime, CommandInfo commandInfo)
    {
        BotId = botId;
        Channel = channel;
        Room = room;
        User = user;
        MessageId = messageId;
        SendTime = sendTime;
        CommandInfo = commandInfo;
    }

    public CommandInfo CommandInfo { get; init; }

    public long BotId { get; init; }
    public Channel Channel { get; init; }
    public Room Room { get; init; }
    public Features.User User { get; init; }
    public string MessageId { get; init; }
    public long SendTime { get; init; }

    public static implicit operator UserSendCommandEventArgs(UserSendCommandData data)
    {
        return new UserSendCommandEventArgs(data.BotId, data.Channel, data.Room, data.User, data.MessageId,
            data.SendTime, data.Command);
    }

    public static implicit operator UserSendCommandEventArgs(ServerMessage<UserSendCommandData> data)
    {
        return data.Data;
    }
}