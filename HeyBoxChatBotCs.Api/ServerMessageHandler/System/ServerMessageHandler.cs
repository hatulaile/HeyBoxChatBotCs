using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.ServerMessageHandler.ServerMessageDatas;

namespace HeyBoxChatBotCs.Api.ServerMessageHandler.System;

internal static class ServerMessageHandler
{
    private static JsonSerializerOptions JsonSerializerOptions { get; } = JsonSerializerOptions.Default;

    public static Dictionary<string, Type> TypeMapping { get; } = new()
    {
        { "50", typeof(ServerMessage<UserSendCommandData>) }
    };

    internal static void ProcessMessage(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        JsonObject? jsonObject = JsonSerializer.Deserialize<JsonObject>(json);
        if (jsonObject is null)
        {
            Log.Error($"解析服务器发送信息时失败:JsonObject为空!");
            return;
        }

        JsonNode? jsonNode = jsonObject["type"];
        if (jsonNode is null)
        {
            Log.Error("解析服务器发送信息时失败:未获取到Type节点!");
            return;
        }

        ProcessMessage(jsonNode.ToString(), json);
    }

    internal static void ProcessMessage(string type, string json)
    {
        if (!TypeMapping.TryGetValue(type, out Type? jsonType))
        {
            Log.Error($"解析服务器发送信息时失败:发现未知服务器信息类型 {type}");
            return;
        }

        if (!Misc.Misc.IsDerivedFromClass(jsonType, typeof(ServerMessage<>), true))
        {
            Log.Error($"解析服务器发送信息时失败: {type} 对应的类型是错误的!");
            return;
        }

        if (jsonType.IsGenericTypeDefinition)
        {
            Log.Error($"解析服务器发送信息时失败: {type} 对应的类型是没有泛型构造!");
            return;
        }

        try
        {
            object? message = JsonSerializer.Deserialize(json, jsonType, JsonSerializerOptions);
            if (message is null)
            {
                Log.Error("解析服务器发送信息时失败:Json解析的结果不是期望结果!");
                return;
            }

            object? data = message.GetType().GetProperty("Data")?.GetValue(message);
            if (data is null)
            {
                Log.Error("解析服务器发送信息时失败:Json解析的结果找不到Data属性!");
                return;
            }

            MethodInfo? invokeEventMethod = data.GetType().GetMethod("InvokeRelatedEvent");
            if (invokeEventMethod is null)
            {
                Log.Error("解析服务器发送信息时失败:Json解析的结果找不到可用方法!");
                return;
            }

            invokeEventMethod.Invoke(data, null);
        }
        catch (Exception ex)
        {
            Log.Error("解析服务器发送信息时错误:" + ex);
        }
    }
}