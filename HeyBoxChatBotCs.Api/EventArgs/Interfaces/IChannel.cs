using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.EventArgs.Interfaces;

public interface IChannel : IEvent
{ 
    Channel Channel { get; init; }
}