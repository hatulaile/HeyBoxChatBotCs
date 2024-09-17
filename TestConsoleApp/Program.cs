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
        new Bot("hatu", "NzIxNzIyODY7MTcyNjQ1MDU5NTA2Nzk0MTY2NDsyNjMzNDE0ODQ2MTMyMTM4ODY=").Start();
    }
}