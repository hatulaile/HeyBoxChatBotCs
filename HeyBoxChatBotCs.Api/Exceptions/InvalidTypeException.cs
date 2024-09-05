namespace HeyBoxChatBotCs.Api.Exceptions;

public class InvalidTypeException : ArgumentException
{
    public InvalidTypeException(string message)
        : base(message)
    {
    }
}