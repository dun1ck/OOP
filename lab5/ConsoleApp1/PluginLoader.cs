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
        {
            Directory.CreateDirectory(folderPath);
            return;
        }

        foreach (string dllPath in Directory.GetFiles(folderPath, "*.dll"))
        {
            if (Path.GetFileName(dllPath).Equals("VehicleCore.dll", StringComparison.OrdinalIgnoreCase))
                continue;
            LoadPluginAssembly(dllPath);
        }
    }

    public static void LoadFromFile(string dllPath)
    {
        if (!File.Exists(dllPath))
        {
            Console.WriteLine("Файл плагина не найден.");
            return;
        }
        LoadPluginAssembly(dllPath);
    }

    private static void LoadPluginAssembly(string dllPath)
    {
        try
        {
            var context = new PluginAssemblyLoadContext(dllPath);
            Assembly assembly = context.LoadFromAssemblyPath(Path.GetFullPath(dllPath));

            foreach (Type type in assembly.GetExportedTypes())
            {
                if (typeof(IVehiclePlugin).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    if (Activator.CreateInstance(type) is IVehiclePlugin vp &&
                        !_vehiclePlugins.Any(p => p.VehicleType == vp.VehicleType))
                    {
                        _vehiclePlugins.Add(vp);
                        Console.WriteLine($"Загружен плагин транспорта: {vp.VehicleTypeName}");
                    }
                }

                if (typeof(IDataProcessingPlugin).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    if (Activator.CreateInstance(type) is IDataProcessingPlugin pp &&
                        !_processingPlugins.Any(p => p.Name == pp.Name))
                    {
                        _processingPlugins.Add(pp);
                        Console.WriteLine($"Загружен плагин обработки: {pp.Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки плагина {dllPath}: {ex.Message}");
        }
    }

    private sealed class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginAssemblyLoadContext(string pluginPath) : base(isCollectible: false) =>
            _resolver = new AssemblyDependencyResolver(pluginPath);

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name == "VehicleCore")
                return typeof(Vehicle).Assembly;

            string? path = _resolver.ResolveAssemblyToPath(assemblyName);
            if (path != null)
                return LoadFromAssemblyPath(path);

            return null;
        }
    }
}
