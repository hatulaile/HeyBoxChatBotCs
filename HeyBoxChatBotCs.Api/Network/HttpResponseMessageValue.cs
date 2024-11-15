namespace HeyBoxChatBotCs.Api.Network;

public class HttpResponseMessageValue<TValue>
{
    public HttpResponseMessageValue(HttpResponseMessage response, TValue value)
    {
        Response = response;
        Value = value;
    }

    public HttpResponseMessage Response { get; init; }
    public TValue Value { get; init; }
}

public class HttpResponseMessageValue
{
    public HttpResponseMessageValue(HttpResponseMessage response, object? value)
    {
        Response = response;
        Value = value;
    }

    public HttpResponseMessage Response { get; init; }
    public object? Value { get; init; }
}