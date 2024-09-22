namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataHandlers;

public interface IDataHandler
{
    public Task ProcessDataAsync(object? serverMessage);
}