using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.Room;

namespace HeyBoxChatBotCs.Api.Events;

public class Room
{
    public static Event<RoomMembershipChangedData> RoomMembershipChanged { get; set; } = new();

    public static Event<RoomMembershipChangedData> UserJoinRoom { get; set; } = new();

    public static Event<RoomMembershipChangedData> UserLeaveRoom { get; set; } = new();

    public static async Task OnRoomMembershipChanged(RoomMembershipChangedData ev)
    {
        await RoomMembershipChanged.InvokeAsync(ev);
    }

    public static async Task OnUserJoinRoom(RoomMembershipChangedData ev)
    {
        await UserJoinRoom.InvokeAsync(ev);
    }

    public static async Task OnUserLeaveRoom(RoomMembershipChangedData ev)
    {
        await UserLeaveRoom.InvokeAsync(ev);
    }
}