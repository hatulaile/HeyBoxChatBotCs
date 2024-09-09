namespace HeyBoxChatBotCs.Api.Commands.CommandSystem;

public static class UserCommandProcessor
{
    public static readonly UserCommandHandler UserCommandHandler = CommandSystem.UserCommandHandler.Create();

    public static void ProcessorInput(string json)
    {
        // // input = input.TrimStart('/', ' ', '\\', '.');
        // // Log.Debug($"用户输入简化为: {input}");
        // // var strings = input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        // if (!UserCommandHandler.TryGetCommand(strings[0], out ICommand? command))
        // {
        //     return;
        // }
        //
        // var args = new ArraySegment<string>(strings, 1, strings.Length - 1);
        // Log.Debug(Misc.IsArrayNullOrEmpty(args) ? "用户无输入参数" : $"参数数组为:{string.Join(',', args)}");
    }
}