using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageData;

namespace HeyBoxChatBotCs.Api;

public delegate Task UserSendCommandAction(UserSendCommandData commandInfo);

public delegate Task ReceiveMessage(string message);

public delegate Task EventHandler(object? sender);