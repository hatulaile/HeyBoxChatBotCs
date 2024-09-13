namespace HeyBoxChatBotCs.Api.Features;

public delegate void CustomEventHandler();

public delegate void CustomAsycnEventHandler();

public class Event
{
    public Event()
    {
        EventsValue.Add(this);
    }

    private event CustomEventHandler? InnerEvent;
    private event CustomAsycnEventHandler? InnerAsyncEvent;
    private static readonly List<Event> EventsValue = [];
    public static IReadOnlyList<Event> List => EventsValue;

    public void Subscribe(CustomEventHandler customEventHandler)
    {
        InnerEvent += customEventHandler;
    }

    public void UnSubscribe(CustomEventHandler customEventHandler)
    {
        InnerEvent -= customEventHandler;
    }

    public static Event operator +(Event e, CustomEventHandler customEventHandler)
    {
        e.Subscribe(customEventHandler);
        return e;
    }

    public static Event operator -(Event e, CustomEventHandler customEventHandler)
    {
        e.UnSubscribe(customEventHandler);
        return e;
    }

    public void Invoke()
    {
        InvokeNormal();
        InvokeAsync();
    }

    internal void InvokeNormal()
    {
        if (InnerEvent is null)
        {
            return;
        }

        foreach (CustomEventHandler handler in InnerEvent.GetInvocationList()
                     .Cast<CustomEventHandler>())
        {
            try
            {
                handler();
            }
            catch (Exception exception)
            {
                Log.Error("触发事件遇到错误:" + exception);
            }
        }
    }

    internal void InvokeAsync()
    {
        if (InnerAsyncEvent is null)
        {
            return;
        }

        foreach (CustomAsycnEventHandler handler in InnerAsyncEvent.GetInvocationList()
                     .Cast<CustomAsycnEventHandler>())
        {
            try
            {
                Task.Run(() => handler());
            }
            catch (Exception exception)
            {
                Log.Error("触发异步事件时遇到错误:" + exception);
            }
        }
    }
}