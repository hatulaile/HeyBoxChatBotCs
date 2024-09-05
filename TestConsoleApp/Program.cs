using HeyBoxChatBotCs.Api.Features.Bot;

namespace TestConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        new Mybot("hatu", "NzIxNzIyODY7MTcyNTUyODM5MDgyOTQ3MzY4MTsxMzk3ODkwMzA5ODcwMzY1MjA4").Start();
    }

    public class Mybot : Bot
    {
        public Mybot(string id, string token) : base(id, token)
        {
        }
    }
}