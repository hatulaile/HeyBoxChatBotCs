using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public static class ConsoleCommandProcessor
{
    public static readonly ConsoleCommandHandler ConsoleCommandHandler = ConsoleCommandHandler.Create();

    public static event ReceiveMessage ConsoleInput = ProcessorInput;

    private static CancellationTokenSource? ConsoleReadCts { get; set; }


    private static void ProcessorInput(string input)
    {
        input = input.TrimStart('/', ' ', '\\', '.');
        Log.Debug($"用户输入简化为: {input}");
        var strings = input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (!ConsoleCommandHandler.TryGetCommand(strings[0], out ICommand? command))
        {
            Log.Warn("未找到输入的命令,可以使用 help 查看所有命令!");
            return;
        }

        var args = new ArraySegment<string>(strings, 1, strings.Length - 1);
        Log.Debug(Misc.IsArrayNullOrEmpty(args) ? "用户无输入参数" : $"参数数组为:{string.Join(',', args)}");
        if (command!.Execute(args, null, out string response))
        {
            Log.Info(string.IsNullOrWhiteSpace(response) ? "执行完毕!" : response);
        }
        else
        {
            Log.Warn(string.IsNullOrWhiteSpace(response) ? "执行失败!" : response);
        }
    }

    internal static void Run()
    {
        if (ConsoleReadCts is { IsCancellationRequested: true })
        {
            Log.Warn("请不要重复启动控制台获取程序!");
            return;
        }

        Log.Debug("控制台已启动");
        ConsoleReadCts = new CancellationTokenSource();
        while (!ConsoleReadCts.IsCancellationRequested)
        {
            string? consoleInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(consoleInput)) continue;
            Log.Debug($"控制台以获取到用户输入: {consoleInput}");
            ConsoleInput?.Invoke(consoleInput);
        }
    }
}