using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using HeyBoxChatBotCs.Api.Extensions;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.Interfaces;

namespace HeyBoxChatBotCs.Api.Loader;

public static class ConfigManager
{
    public static JsonSerializerOptions? ConfigJsonSerializerOptions { get; } = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    public static void Reload()
    {
        Load(Read());
        Save();
    }

    private static void Save()
    {
        foreach (IPlugin<IConfig> plugin in Loader.Plugins)
        {
            string configPath = Paths.GetPluginConfigPath(plugin);
            File.WriteAllText(configPath,
                JsonSerializer.Serialize(plugin.Config, plugin.Config.GetType(), ConfigJsonSerializerOptions));
        }
    }

    private static void Load(List<Dictionary<string, object?>> configs)
    {
        for (int i = 0; i < Loader.Plugins.Count; i++)
        {
            Loader.Plugins.ElementAt(i).Config.CopyProperties(configs[i]);
        }
    }

    private static List<Dictionary<string, object?>> Read()
    {
        List<Dictionary<string, object?>> pluginConfig = new(5);
        foreach (IPlugin<IConfig> plugin in Loader.Plugins)
        {
            string configPath = Paths.GetPluginConfigPath(plugin);
            if (!File.Exists(configPath))
            {
                Log.Info($"{plugin.Name} 插件不存在配置文件,将新建一个配置文件!");
                pluginConfig.Add(plugin.Config.PropertiesToDictionary());
            }
            else
            {
                try
                {
                    pluginConfig.Add(JsonSerializer.Deserialize(File.ReadAllText(configPath), plugin.Config.GetType(),
                            ConfigJsonSerializerOptions)
                        .PropertiesToDictionary());
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    Log.Error("由于解析错误,将源文件备份到配置文件夹里!");
                    File.WriteAllText(configPath + ".old", File.ReadAllText(configPath));
                    pluginConfig.Add(plugin.Config.PropertiesToDictionary());
                }
            }
        }

        return pluginConfig;
    }
}