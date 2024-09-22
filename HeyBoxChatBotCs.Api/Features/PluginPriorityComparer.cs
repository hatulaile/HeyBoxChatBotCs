using HeyBoxChatBotCs.Api.Interfaces;

namespace HeyBoxChatBotCs.Api.Features
{
    public sealed class PluginPriorityComparer : IComparer<IPlugin<IConfig>>
    {
        public static readonly PluginPriorityComparer Instance = new();

        public int Compare(IPlugin<IConfig>? x, IPlugin<IConfig>? y)
        {
            if (x is null || y is null)
            {
                return 0;
            }

            int value = y.Priority.CompareTo(x.Priority);
            if (value == 0)
                value = x.GetHashCode().CompareTo(y.GetHashCode());
            return value == 0 ? 1 : value;
        }
    }
}