namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CommandHandlerAttribute : Attribute
{
    public CommandHandlerAttribute(Type type)
    {
        if (!Misc.Misc.IsDerivedFromClass<CommandHandler>(type))
        {
            throw new ArgumentException($"这个类型不是派生于{nameof(CommandHandler)}", type.Name);
        }
    }
}