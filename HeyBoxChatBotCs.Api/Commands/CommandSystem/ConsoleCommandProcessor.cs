﻿using HeyBoxChatBotCs.Api.Commands.Interfaces;
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
        await ProcessorInput(input);
    }


    private static async Task ProcessorInput(string input)
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

    internal static async Task Run()
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
            string? consoleInput = await Console.In.ReadLineAsync(ConsoleReadCts.Token);
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