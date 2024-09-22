using HeyBoxChatBotCs.Api.Interfaces;

namespace HeyBoxChatBotCs.Api.Features;

public static class Paths
{
    static Paths() => Reload();

    public static string RootPath { get; set; } = null!;
    public static string ConfigPath { get; set; } = null!;
    public static string PluginPath { get; set; } = null!;
    public static string DependenciesPath { get; set; } = null!;

    public static void Reload()
    {
        RootPath = Environment.CurrentDirectory;
        ConfigPath = Path.Combine(RootPath, "Configs");
        IfNotExistsBeCreate(ConfigPath);
        DependenciesPath = Path.Combine(RootPath, "Dependencies");
        IfNotExistsBeCreate(DependenciesPath);
        PluginPath = Path.Combine(RootPath, "Plugins");
        IfNotExistsBeCreate(PluginPath);
    }

    /// <summary>
    /// 获取此插件的配置文件路径
    /// </summary>
    /// <param name="plugin">要获取的插件</param>
    /// <typeparam name="TConfig">配置文件类</typeparam>
    /// <returns></returns>
    public static string GetPluginConfigPath<TConfig>(IPlugin<TConfig> plugin)
        where TConfig : IConfig
    {
        var path = Path.Combine(ConfigPath, ClearInvalidChars(Bot.Bot.Instance?.Id ?? "Default"));
        IfNotExistsBeCreate(path);
        return Path.Combine(path, ClearInvalidChars(plugin.Name) + ".json");
    }

    public static string ClearInvalidChars(string fileName, char newChar = '_')
    {
        return ClearInvalidChars(fileName.ToCharArray(), newChar);
    }

    public static string ClearInvalidChars(Span<char> fileName, char newChar = '_')
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        if (invalidChars.Contains(newChar))
        {
            Log.Error($"{newChar} 不是一个合法的文件名称!");
            newChar = '_';
        }

        for (int i = 0; i < fileName.Length; i++)
        {
            if (!invalidChars.Contains(fileName[i]))
            {
                continue;
            }

            fileName[i] = newChar;
        }

        return fileName.ToString();
    }

    /// <summary>
    /// 如果不存在此文件夹就创造它
    /// </summary>
    /// <param name="directoryPath">要创造的文件夹路径</param>
    /// <returns>如果成功创造则返回 <c>true</c> 否则返回 <c>false</c></returns>
    public static bool IfNotExistsBeCreate(string directoryPath)
    {
        try
        {
            if (Directory.Exists(directoryPath))
            {
                return false;
            }

            Directory.CreateDirectory(directoryPath);
            return true;
        }
        catch (Exception e)
        {
            Log.Error(e);
            return false;
        }
    }
}