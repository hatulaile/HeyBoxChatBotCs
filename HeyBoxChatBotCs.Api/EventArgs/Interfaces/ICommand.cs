using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface ICommand : IEvent
{
    CommandInfo CommandInfo { get; init; }
}