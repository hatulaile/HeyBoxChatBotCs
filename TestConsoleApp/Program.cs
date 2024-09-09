using HeyBoxChatBotCs.Api.Features.Bot;

namespace TestConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        new Bot("hatu", "NzIxNzIyODY7MTcyNTUyODM5MDgyOTQ3MzY4MTsxMzk3ODkwMzA5ODcwMzY1MjA4").Start();
    }
}