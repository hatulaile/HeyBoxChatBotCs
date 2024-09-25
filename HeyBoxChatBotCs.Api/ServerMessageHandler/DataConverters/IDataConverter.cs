namespace HeyBoxChatBotCs.Api.ServerMessageHandler.DataConverters;

public interface IDataConverter
{
    Task<object?> ConverterAsync(string message);
}