namespace HeyBoxChatBotCs.Api.Enums;

public enum RoleType : byte
{
    Default = 0,
    Game = 1,
    Bot = 2,
    User = 3,
    TextAdmin = 4,
    VoiceAdmin = 5,
    CommunityBuilder = 6,
    SuperAdmin = 7,
    Visitors = 254,
    All = 255,
}