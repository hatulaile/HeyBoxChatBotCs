namespace HeyBoxChatBotCs.Api.Features;

public delegate Task CustomAsyncEventHandler<in TEventArgs>(TEventArgs args);

public class Event<TEventArgs>
{
    private static readonly Dictionary<Type, List<Event<TEventArgs>>> TypeToEvent = new();

    public Event()
    {
        if (!TypeToEvent.ContainsKey(typeof(TEventArgs)))
        {
            TypeToEvent[typeof(TEventArgs)] = [this];
        }
        else
        {
            TypeToEvent[typeof(TEventArgs)].Add(this);
        }
    }

    public static IReadOnlyDictionary<Type, List<Event<TEventArgs>>> Dictionary => TypeToEvent;
    private event CustomAsyncEventHandler<TEventArgs>? InnerAsyncEvent;

    public void Subscribe(CustomAsyncEventHandler<TEventArgs> customEventHandler)
    {
        InnerAsyncEvent += customEventHandler;
    }

    public void UnSubscribe(CustomAsyncEventHandler<TEventArgs> customEventHandler)
    {
        InnerAsyncEvent -= customEventHandler;
    }

    public static Event<TEventArgs> operator +(Event<TEventArgs> e,
        CustomAsyncEventHandler<TEventArgs> customEventHandler)
    {
        e.Subscribe(customEventHandler);
        return e;
    }

    public static Event<TEventArgs> operator -(Event<TEventArgs> e,
        CustomAsyncEventHandler<TEventArgs> customEventHandler)
    {
        e.UnSubscribe(customEventHandler);
        return e;
    }

    public async Task InvokeAsync(TEventArgs args)
    {
        if (InnerAsyncEvent is null)
        {
            return;
        }

        foreach (CustomAsyncEventHandler<TEventArgs> handler in InnerAsyncEvent.GetInvocationList()
                     .Cast<CustomAsyncEventHandler<TEventArgs>>())
        {
            try
            {
                await handler(args);
            }
            catch (Exception exception)
            {
                Log.Error("触发事件时遇到错误:" + exception);
            }
        }
    }
}