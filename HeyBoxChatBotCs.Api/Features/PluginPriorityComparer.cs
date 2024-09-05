using HeyBoxBotCs.Api.Interfaces;

namespace HeyBoxBotCs.Api.Features
{
    public sealed class PluginPriorityComparer : IComparer<IPlugin<IConfig>>
    {
        /// <summary>
        /// Public instance.
        /// </summary>
        public static readonly PluginPriorityComparer Instance = new();

        /// <inheritdoc/>
        public int Compare(IPlugin<IConfig>? x, IPlugin<IConfig>? y)
        {
            int value = y.Priority.CompareTo(x.Priority);
            if (value == 0)
                value = x.GetHashCode().CompareTo(y.GetHashCode());
            return value == 0 ? 1 : value;
        }
    }
}