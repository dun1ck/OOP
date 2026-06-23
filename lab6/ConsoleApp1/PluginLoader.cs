using System.Reflection;
using System.Runtime.Loader;
using VehicleCore;

namespace ConsoleApp1;

public static class PluginLoader
{
    private static readonly List<IVehiclePlugin> _vehiclePlugins = new();
    private static readonly List<IDataProcessingPlugin> _processingPlugins = new();

    public static IReadOnlyList<IVehiclePlugin> VehiclePlugins => _vehiclePlugins;
    public static IReadOnlyList<IDataProcessingPlugin> ProcessingPlugins => _processingPlugins;

    public static void LoadFromFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        foreach (string dll in Directory.GetFiles(folderPath, "*.dll"))
        {
            if (Path.GetFileName(dll).Equals("VehicleCore.dll", StringComparison.OrdinalIgnoreCase))
                continue;
            LoadPluginAssembly(dll, useAdapter: false);
        }
    }

    public static void LoadFriendPlugins(string friendFolder)
    {
        if (!Directory.Exists(friendFolder))
            Directory.CreateDirectory(friendFolder);

        foreach (string dll in Directory.GetFiles(friendFolder, "*.dll"))
            LoadPluginAssembly(dll, useAdapter: true);
    }

    public static void LoadFromFile(string dllPath, bool asFriendPlugin = false)
    {
        if (!File.Exists(dllPath))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }
        LoadPluginAssembly(dllPath, useAdapter: asFriendPlugin);
    }

    private static void LoadPluginAssembly(string dllPath, bool useAdapter)
    {
        try
        {
            var context = new PluginAssemblyLoadContext(dllPath);
            Assembly assembly = context.LoadFromAssemblyPath(Path.GetFullPath(dllPath));

            foreach (Type type in assembly.GetExportedTypes())
            {
                if (!useAdapter &&
                    typeof(IDataProcessingPlugin).IsAssignableFrom(type) &&
                    !type.IsAbstract && !type.IsInterface)
                {
                    if (Activator.CreateInstance(type) is IDataProcessingPlugin native &&
                        !_processingPlugins.Any(p => p.Name == native.Name))
                    {
                        _processingPlugins.Add(native);
                        Console.WriteLine($"Плагин обработки: {native.Name}");
                    }
                }

                if (useAdapter &&
                    typeof(IFriendDataProcessor).IsAssignableFrom(type) &&
                    !type.IsAbstract && !type.IsInterface)
                {
                    if (Activator.CreateInstance(type) is IFriendDataProcessor friend)
                    {
                        var adapted = new FriendPluginAdapter(friend);
                        if (!_processingPlugins.Any(p => p.Name == adapted.Name))
                        {
                            _processingPlugins.Add(adapted);
                            Console.WriteLine($"Адаптирован плагин товарища: {adapted.Name}");
                        }
                    }
                }

                if (typeof(IVehiclePlugin).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    if (Activator.CreateInstance(type) is IVehiclePlugin vp &&
                        !_vehiclePlugins.Any(p => p.VehicleType == vp.VehicleType))
                    {
                        _vehiclePlugins.Add(vp);
                        Console.WriteLine($"Плагин транспорта: {vp.VehicleTypeName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки {dllPath}: {ex.Message}");
        }
    }

    private sealed class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;
        public PluginAssemblyLoadContext(string path) : base(isCollectible: false) =>
            _resolver = new AssemblyDependencyResolver(path);

        protected override Assembly? Load(AssemblyName name)
        {
            if (name.Name == "VehicleCore")
                return typeof(Vehicle).Assembly;

            string? path = _resolver.ResolveAssemblyToPath(name);
            if (path != null)
                return LoadFromAssemblyPath(path);

            return null;
        }
    }
}
