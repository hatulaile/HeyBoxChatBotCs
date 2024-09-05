namespace HeyBoxBotCs.Api.Features;

public delegate void CustomEventHandler();

public class Event
{
    public Event()
    {
        EventsValue.Add(this);
    }

    private event CustomEventHandler? InnerEvent;
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
        InnerEvent?.Invoke();
    }
}