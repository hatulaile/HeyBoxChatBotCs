using System.Reflection;
using HeyBoxChatBotCs.Api.Features;
using HeyBoxChatBotCs.Api.Interfaces;

namespace HeyBoxChatBotCs.Api.Loader;

public class Loader
{
    public static Dictionary<Assembly, string> Locations { get; } = new();
    public static List<Assembly> LoadedAssembly => Locations.Keys.ToList();

    public static SortedSet<IPlugin<IConfig>> Plugins { get; } = new(PluginPriorityComparer.Instance);

    public static readonly HashSet<Assembly> Dependencies = [];

    public Loader()
    {
#if DEBUG
        Log.Error("目前处于DEBUG模式,如需生产使用请使用RELEASE模式");
#endif
        Log.Info($"正在初始化,文件夹位于:{Paths.RootPath}");
        AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
    }

    private static Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args) =>
        LoadedAssembly.FirstOrDefault(assembly => assembly.FullName == args.Name);

    public async Task Run()
    {
        await LoadDependencies();
        await LoadPlugins();
        ConfigManager.Reload();
        await EnablePlugins();
    }

    public static Task<Assembly?> LoadAssembly(string path)
    {
        try
        {
            Assembly assembly = Assembly.LoadFile(path);
            return Task.FromResult(assembly)!;
        }
        catch (Exception exception)
        {
            Log.Error($"在加载 {path} 程序集时出错! {exception}");
        }

        return Task.FromResult<Assembly?>(null);
    }

    public static async Task LoadPlugins()
    {
        Log.Info("开始加载插件~");
        foreach (string filePath in Directory.GetFiles(Paths.PluginPath, "*.dll"))
        {
            Assembly? assembly = await LoadAssembly(filePath);
            if (assembly is null)
            {
                continue;
            }

            Locations[assembly] = filePath;
        }

        foreach (Assembly assembly in LoadedAssembly.Where(x => !Dependencies.Contains(x)))
        {
            IPlugin<IConfig>? plugin = await CreatePlugin(assembly);
            if (plugin is null)
                continue;
            AssemblyInformationalVersionAttribute? attribute =
                plugin.Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            Log.Info(
                $"加载插件 {plugin.Name}@{(plugin.Version is not null ? $"{plugin.Version.Major}.{plugin.Version.Minor}.{plugin.Version.Build}" : attribute is not null ? attribute.InformationalVersion : string.Empty)}");
            Plugins.Add(plugin);
        }

        Log.Info("插件加载完毕~");
    }

    public static Task<IPlugin<IConfig>?> CreatePlugin(Assembly assembly)
    {
        try
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract)
                {
                    Log.Debug($"\"{type.FullName}\"是抽象类,跳过!");
                    continue;
                }

                if (!Misc.Misc.IsDerivedFromClass(type, typeof(Plugin<>), true))
                {
                    Log.Debug($"\"{type.FullName}\"不是不是一个插件,跳过!");
                    continue;
                }

                Log.Debug($"加载插件 {type.FullName}");

                IPlugin<IConfig>? plugin = null;
                ConstructorInfo? constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor is not null)
                {
                    Log.Debug($"已找到公开构造函数,正在实例化 {type.FullName} 类型!");
                    plugin = constructor.Invoke([]) as IPlugin<IConfig>;
                }
                else
                {
                    Log.Debug($"未找到公开无参构造函数,正在寻找可用的 {type.FullName} 类型!");
                    object? value = type
                        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        .FirstOrDefault(properties => properties.PropertyType == type)?.GetValue(null);
                    if (value is not null)
                    {
                        plugin = value as IPlugin<IConfig>;
                    }
                }

                if (plugin is null)
                {
                    Log.Error($"类型 {type.FullName} 是个可用插件,但是未找到可用的入口点!");
                    continue;
                }

                Log.Debug($"{type.FullName} 已经实例化完成!");
                return Task.FromResult(plugin)!;
            }
        }
        catch (ReflectionTypeLoadException reflectionTypeLoadException)
        {
            Log.Error(
                $"初始化插件时遇到错误 {assembly.GetName().Name} (at {assembly.Location})! {reflectionTypeLoadException}");

            foreach (Exception? loaderException in reflectionTypeLoadException.LoaderExceptions)
            {
                if (loaderException is null)
                {
                    continue;
                }

                Log.Error(loaderException);
            }
        }
        catch (Exception exception)
        {
            Log.Error($"初始化插件时遇到错误 {assembly.GetName().Name} (at {assembly.Location})! {exception}");
        }

        return Task.FromResult<IPlugin<IConfig>?>(null);
    }


    public static async Task LoadDependencies()
    {
        try
        {
            Log.Info($"加载位于 {Paths.DependenciesPath} 的依赖程序集!");
            foreach (string filePath in Directory.GetFiles(Paths.DependenciesPath, "*.dll"))
            {
                Assembly? assembly = await LoadAssembly(filePath);
                if (assembly is null)
                {
                    continue;
                }

                Locations[assembly] = filePath;
                Dependencies.Add(assembly);
                Log.Info($"成功加载附属程序集 {assembly.GetName().Name}@{assembly.GetName().Version!.ToString(3)}");
            }

            Log.Info("程序集加载完成!");
        }
        catch (Exception e)
        {
            Log.Error($"加载程序集时遇到以下错误:{e}");
        }
    }


    public static Task DisablePlugins()
    {
        foreach (IPlugin<IConfig> plugin in Plugins)
        {
            try
            {
                plugin.OnDisabled();
                plugin.Config.IsEnabled = false;
                plugin.OnUnregisteringCommands();
            }
            catch (Exception ex)
            {
                Log.Error("禁用插件时遇到错误:" + ex);
            }
        }

        return Task.CompletedTask;
    }

    public static async Task ReloadPlugins()
    {
        foreach (IPlugin<IConfig> plugin in Plugins)
        {
            try
            {
                plugin.OnReloaded();
            }
            catch (Exception exception)
            {
                Log.Error("重启插件遇到错误:" + exception);
            }
        }

        await DisablePlugins();
        Plugins.Clear();
        Locations.Clear();
        await LoadPlugins();
        ConfigManager.Reload();
        await EnablePlugins();
    }

    public static Task EnablePlugins()
    {
        List<IPlugin<IConfig>> toLoad = Plugins.ToList();
        foreach (IPlugin<IConfig> plugin in toLoad.Where(p => p.Name.StartsWith("HeyBoxBotCs") && p.Config.IsEnabled)
                     .ToList())
        {
            try
            {
                plugin.OnEnabled();
                plugin.OnRegisteringCommands();
                toLoad.Remove(plugin);
                if (plugin.Config.IsDebug)
                {
                    Log.DebugEnabled.Add(plugin.Assembly);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        foreach (IPlugin<IConfig> plugin in toLoad)
        {
            if (plugin.Config.IsEnabled)
            {
                plugin.OnEnabled();
                plugin.OnRegisteringCommands();
            }

            if (plugin.Config.IsDebug)
            {
                Log.DebugEnabled.Add(plugin.Assembly);
            }
        }

        return Task.CompletedTask;
    }
}