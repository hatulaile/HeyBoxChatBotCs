namespace HeyBoxChatBotCs.Api;

public static class Misc
{
    public static string GetNowTimeString()
    {
        DateTime nowTime = DateTime.Now;
        return $"{nowTime:G} UTF{nowTime:zz}";
    }

    public static bool IsArrayNullOrEmpty<T>(T[]? array) => array is null || array.Length == 0;

    public static bool IsDerivedFromClass<TType>(object source)
    {
        return IsDerivedFromClass(source.GetType(), typeof(TType));
    }

    public static bool IsDerivedFromClass(object source, Type? type)
    {
        return IsDerivedFromClass(source.GetType(), type);
    }

    public static bool IsDerivedFromClass(Type? source, Type? target, bool isDefinition = false)
    {
        if (source is null && target is null)
        {
            return true;
        }

        if (source is null || target is null)
        {
            return false;
        }

        if (!target.IsGenericType)
        {
            isDefinition = false;
        }

        while (source is not null)
        {
            source = source.BaseType;
            if (source is null)
            {
                continue;
            }

            if (source == target)
            {
                return true;
            }
            else if (isDefinition && source.IsGenericType && target.IsGenericType)
            {
                if (target.GetGenericTypeDefinition() == source.GetGenericTypeDefinition())
                {
                    return true;
                }
            }
        }

        return false;
    }
}