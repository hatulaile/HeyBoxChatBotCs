using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageDatas;

namespace HeyBoxChatBotCs.Api;


public delegate void UserSendCommandAction(UserSendCommandData commandInfo);
public delegate void ReceiveMessage(string message);

public delegate void EventHandler(object? sender);