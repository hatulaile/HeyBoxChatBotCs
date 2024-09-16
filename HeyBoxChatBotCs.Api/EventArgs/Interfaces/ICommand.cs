using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface ICommand : IEvent
{
    public CommandInfo CommandInfo { get; init; }
}