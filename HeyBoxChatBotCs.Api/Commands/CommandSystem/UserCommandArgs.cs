using HeyBoxChatBotCs.Api.EventArgs.Interfaces;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public class UserCommandArgs
{
    public UserCommandArgs(string messageId, long sendTime, CommandInfo commandInfo, Channel channel, Room room)
    {
        MessageId = messageId;
        SendTime = sendTime;
        CommandInfo = commandInfo;
        Channel = channel;
        Room = room;
    }

    public string MessageId { get; init; }
    public long SendTime { get; init; }
    public CommandInfo CommandInfo { get; init; }
    public Channel Channel { get; init; }
    public Room Room { get; init; }
    public CommandOption[]? Option => CommandInfo.Options;
}