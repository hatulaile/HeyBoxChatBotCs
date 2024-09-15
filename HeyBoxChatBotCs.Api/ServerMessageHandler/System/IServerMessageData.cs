using System.Net.Mime;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.System;

public interface IServerMessageData
{
    public void InvokeRelatedEvent();
}