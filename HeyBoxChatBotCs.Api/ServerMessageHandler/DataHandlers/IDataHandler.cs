namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataHandlers;

public interface IDataHandler
{
    Task ProcessDataAsync(object? serverMessage);
}