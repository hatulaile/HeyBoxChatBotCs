namespace HeyBoxChatBotCs.Api.Features;

public delegate Task CustomAsyncEventHandler();

public class Event
{
    private static readonly List<Event> EventsValue = [];

    public Event()
    {
        EventsValue.Add(this);
    }

    public static IReadOnlyList<Event> List => EventsValue;

    private event CustomAsyncEventHandler? InnerAsyncEvent;

    public void Subscribe(CustomAsyncEventHandler customEventHandler)
    {
        InnerAsyncEvent += customEventHandler;
    }

    public void UnSubscribe(CustomAsyncEventHandler customEventHandler)
    {
        InnerAsyncEvent -= customEventHandler;
    }

    public static Event operator +(Event e, CustomAsyncEventHandler customEventHandler)
    {
        e.Subscribe(customEventHandler);
        return e;
    }

    public static Event operator -(Event e, CustomAsyncEventHandler customEventHandler)
    {
        e.UnSubscribe(customEventHandler);
        return e;
    }


    internal async Task InvokeAsync()
    {
        if (InnerAsyncEvent is null)
        {
            return;
        }

        foreach (CustomAsyncEventHandler handler in InnerAsyncEvent.GetInvocationList()
                     .Cast<CustomAsyncEventHandler>())
        {
            try
            {
                await handler().WaitAsync(TimeSpan.FromSeconds(10d));
            }
            catch (TimeoutException timeoutException)
            {
                Log.Error("异步事件超时:" + timeoutException);
            }
            catch (Exception exception)
            {
                Log.Error("触发异步事件时遇到错误:" + exception);
            }
        }
    }
}