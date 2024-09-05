namespace HeyBoxBotCs.Api;

public static class Misc
{
    public static string GetNowTimeString()
    {
        DateTime nowTime = DateTime.Now;
        return $"{nowTime:G} UTF{nowTime:zz}";
    }
}