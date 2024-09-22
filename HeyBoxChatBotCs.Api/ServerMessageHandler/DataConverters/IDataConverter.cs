namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataConverters;

public interface IDataConverter
{
    public Task<object?> ConverterAsync(string message);
}