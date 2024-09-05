using System.Reflection;
using HeyBoxBotCs.Api.Exceptions;
using HeyBoxBotCs.Api.Features;

namespace HeyBoxBotCs.Api.Extensions;

public static class ReflectionExtensions
{
    public static void CopyProperties(this object target, IReadOnlyDictionary<string, object?> source)
    {
        Type type = target.GetType();
        foreach (PropertyInfo info in type.GetProperties())
        {
            var t = source.FirstOrDefault(x => x.Key == info.Name).Value;
            if (t is null)
            {
                Log.Warn($"复制属性时未找到 {info.Name} 属性!");
                continue;
            }

            info.SetValue(target, t);
        }
    }

    public static Dictionary<string, object?> PropertiesToDictionary(this object o)
    {
        ArgumentNullException.ThrowIfNull(o);
        return o.GetType().GetProperties().ToDictionary(info => info.Name, info => info.GetValue(o));
    }
}