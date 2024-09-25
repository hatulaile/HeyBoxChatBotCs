using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Nodes;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.ServerMessageHandler.DataConverters;
using HeyBoxChatBotCs.Api.ServerMessageHandler.DataHandlers;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.System;

internal static class ServerMessageHandler
{
    private static JsonSerializerOptions JsonSerializerOptions { get; } = JsonSerializerOptions.Default;

    public static FrozenDictionary<string, KeyValuePair<IDataConverter, IDataHandler>> HandlerMapping { get; } =
        new Dictionary<string, KeyValuePair<IDataConverter, IDataHandler>>
        {
            {
                "50",
                new KeyValuePair<IDataConverter, IDataHandler>(new UserSendCommandConverter(),
                    new UserSendCommandHandler())
            }
        }.ToFrozenDictionary();

    internal static async Task ProcessMessageAsync(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        var jsonObject = JsonSerializer.Deserialize<JsonObject>(json);
        if (jsonObject is null)
        {
            Log.Error("解析服务器发送信息时失败:JsonObject为空!");
            return;
        }

        JsonNode? jsonNode = jsonObject["type"];
        if (jsonNode is null)
        {
            Log.Error("解析服务器发送信息时失败:未获取到Type节点!");
            return;
        }

        await ProcessMessageAsync(jsonNode.ToString(), json);
    }

    internal static async Task ProcessMessageAsync(string typeStr, string json)
    {
        if (!HandlerMapping.TryGetValue(typeStr, out KeyValuePair<IDataConverter, IDataHandler> type))
        {
            Log.Error($"解析服务器发送信息时失败:发现未知服务器信息类型 {typeStr}");
            return;
        }

        try
        {
            object? message = await type.Key.ConverterAsync(json);
            if (message is null)
            {
                Log.Error("解析服务器发送信息时失败:Json解析的结果不是期望结果!");
                return;
            }

            await type.Value.ProcessDataAsync(message);
        }
        catch (Exception ex)
        {
            Log.Error("解析服务器发送信息时错误:" + ex);
        }
    }
}