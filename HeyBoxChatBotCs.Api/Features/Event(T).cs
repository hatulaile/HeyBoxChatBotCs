namespace HeyBoxChatBotCs.Api.Features;

public delegate Task CustomAsyncEventHandler<in TEventArgs>(TEventArgs args);

public class Event<TEventArgs>
{
    private static readonly Dictionary<Type, Event<TEventArgs>> TypeToEvent = new();

    public Event()
    {
        TypeToEvent.Add(typeof(TEventArgs), this);
    }

    public static IReadOnlyDictionary<Type, Event<TEventArgs>> Dictionary => TypeToEvent;
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
                Log.Error("触发异步事件时遇到错误:" + exception);
            }
        }
    }
}