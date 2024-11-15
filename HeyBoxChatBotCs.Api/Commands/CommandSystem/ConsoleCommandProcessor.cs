using HeyBoxChatBotCs.Api.Commands.Interfaces;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public static class ConsoleCommandProcessor
{
    public static readonly ConsoleCommandHandler ConsoleCommandHandler = ConsoleCommandHandler.Create();

    internal static CancellationTokenSource? ConsoleReadCts { get; set; }

    public static event ReceiveMessage? ConsoleInput;

    internal static async Task InvokeConsoleCommandAsync(string input)
    {
        ConsoleInput?.Invoke(input);
        await ProcessorInputAsync(input).ConfigureAwait(false);
    }


    private static async Task ProcessorInputAsync(string input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            input = input.TrimStart('/', ' ', '\\', '.');
            Log.Debug($"控制台输入简化为: {input}");
            string[] inputs = input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (!ConsoleCommandHandler.TryGetCommand(inputs[0], out ICommandBase? command))
            {
                Log.Warn("未找到输入的命令,可以使用 help 查看所有命令!");
                return;
            }

            if (command is not IConsoleCommand consoleCommand)
            {
                Log.Error($"错误的传入了命令,应为{nameof(IConsoleCommand)}!");
                return;
            }

            var args = new ArraySegment<string>(inputs, 1, inputs.Length - 1);
            Log.Debug(Misc.Misc.IsArrayNullOrEmpty(args) ? "控制台无输入参数" : $"参数数组为:{string.Join(',', args)}");
            string response = await consoleCommand.Execute(args);
            Log.Info(string.IsNullOrWhiteSpace(response) ? "执行完毕!" : response);
        }
        catch (Exception ex)
        {
            Log.Error("处理控制台命令发生错误:" + ex);
        }
    }

    internal static async Task RunAsync()
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
            string? consoleInput = await Console.In.ReadLineAsync(ConsoleReadCts.Token).ConfigureAwait(false);
            if (ConsoleReadCts.IsCancellationRequested)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(consoleInput)) continue;
            Log.Debug($"控制台以获取到用户输入: {consoleInput}");
            await InvokeConsoleCommandAsync(consoleInput);
        }
    }
}