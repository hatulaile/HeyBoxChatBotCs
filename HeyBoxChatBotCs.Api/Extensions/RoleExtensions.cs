using HeyBoxChatBotCs.Api.Enums;
using HeyBoxChatBotCs.Api.Features;

namespace HeyBoxChatBotCs.Api.Extensions;

public static class RoleExtensions
{
    public static bool CheckPermission(this Role role, Permission permission)
    {
        return role.Permission.HasFlag(Permission.Admin) || role.Permission.HasFlag(permission);
    }
}