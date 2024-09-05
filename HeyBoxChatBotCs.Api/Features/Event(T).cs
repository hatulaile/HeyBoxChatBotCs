namespace HeyBoxChatBotCs.Api.Features;

public delegate void CustomEventHandler<in TEventArgs>(TEventArgs args);

public class Event<TEventArgs>
{
    private event CustomEventHandler<TEventArgs>? InnerEvent;

    public Event()
    {
        TypeToEvent.Add(typeof(TEventArgs), this);
    }

    private static readonly Dictionary<Type, Event<TEventArgs>> TypeToEvent = new();
    public static IReadOnlyDictionary<Type, Event<TEventArgs>> Dictionary => TypeToEvent;

    public void Subscribe(CustomEventHandler<TEventArgs> customEventHandler)
    {
        InnerEvent += customEventHandler;
    }

    public void UnSubscribe(CustomEventHandler<TEventArgs> customEventHandler)
    {
        InnerEvent -= customEventHandler;
    }

    public static Event<TEventArgs> operator +(Event<TEventArgs> e, CustomEventHandler<TEventArgs> customEventHandler)
    {
        e.Subscribe(customEventHandler);
        return e;
    }

    public static Event<TEventArgs> operator -(Event<TEventArgs> e, CustomEventHandler<TEventArgs> customEventHandler)
    {
        e.UnSubscribe(customEventHandler);
        return e;
    }

    public void Invoke(TEventArgs args)
    {
        InnerEvent?.Invoke(args);
    }
}