using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.Features.Bot;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageDatas;
using HeyBoxChatBotCs.Api.ServerMessageHandler.System;

namespace TestConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        // ServerMessage<UserSendCommandData> data = new ServerMessage<UserSendCommandData>();
        // Log.Info(data is IServerMessage<IServerMessageData>);
        new Bot("hatu", "NzIxNzIyODY7MTcyNTUyODM5MDgyOTQ3MzY4MTsxMzk3ODkwMzA5ODcwMzY1MjA4").Start();
    }
}