using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData.Room;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataHandlers.Room;

public class RoomMembershipChangedHandler : IDataHandler
{
    public async Task ProcessDataAsync(object? serverMessage)
    {
        if (serverMessage is not RoomMembershipChangedData roomMembershipChangedData)
        {
            return;
        }

        await Events.Room.OnRoomMembershipChanged(roomMembershipChangedData);
        if (roomMembershipChangedData.State == RoomMembershipChangedTypeId.Join)
        {
            await Events.Room.OnUserJoinRoom(roomMembershipChangedData);
        }
        else if (roomMembershipChangedData.State == RoomMembershipChangedTypeId.Leave)
        {
            await Events.Room.OnUserLeaveRoom(roomMembershipChangedData);
        }
    }
}