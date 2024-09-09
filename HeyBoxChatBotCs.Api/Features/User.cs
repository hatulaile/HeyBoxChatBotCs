using System.Text.Json.Serialization;
using HeyBoxChatBotCs.Api.Commands.Interfaces;

namespace HeyBoxChatBotCs.Api.Features;

public class User : ICommandSender
{
    [JsonPropertyName("user_base_info")] public UserInfo UserInfo { get; private set; }


    [JsonIgnore] public string Name => UserInfo.Name;
    [JsonIgnore] public string NickName => UserInfo.NickName;
    [JsonIgnore] public long UserId => UserInfo.UserId;
    [JsonIgnore] public bool IsBot => UserInfo.IsBot;
    [JsonIgnore] public int Level => UserInfo.Level;

    public static User? Get(ICommandSender sender)
    {
        return sender as User;
    }
}

public class UserInfo
{
    [JsonPropertyName("nickname")] public string Name { get; private set; }
    [JsonPropertyName("user_id")] public long UserId { get; }
    [JsonPropertyName("bot")] public bool IsBot { get; private set; }
    [JsonPropertyName("level")] public int Level { get; private set; }
    [JsonPropertyName("roles")] public string[] Roles { get; private set; }
    [JsonPropertyName("room_nickname")] public string NickName { get; private set; }
}