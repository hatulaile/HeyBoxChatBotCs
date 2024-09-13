namespace HeyBoxChatBotCs.Api.Features;

public delegate void CustomEventHandler<in TEventArgs>(TEventArgs args);

public delegate void CustomAsyncEventHandler<in TEventArgs>(TEventArgs args);

public class Event<TEventArgs>
{
    private event CustomEventHandler<TEventArgs>? InnerEvent;
    private event CustomAsyncEventHandler<TEventArgs>? InnerAsyncEvent;

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
        InvokeNormal(args);
        InvokeAsync(args);
    }

    internal void InvokeNormal(TEventArgs args)
    {
        if (InnerEvent is null)
        {
            return;
        }

        foreach (CustomEventHandler<TEventArgs> handler in InnerEvent.GetInvocationList()
                     .Cast<CustomEventHandler<TEventArgs>>())
        {
            try
            {
                handler(args);
            }
            catch (Exception exception)
            {
                Log.Error("触发事件遇到错误:" + exception);
            }
        }
    }

    internal void InvokeAsync(TEventArgs args)
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
                Task.Run(() => handler(args));
            }
            catch (Exception exception)
            {
                Log.Error("触发异步事件时遇到错误:" + exception);
            }
        }
    }
}