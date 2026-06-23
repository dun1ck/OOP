using System.Reflection;
using System.Runtime.Loader;
using VehicleCore;

namespace ConsoleApp1;

public static class PluginLoader
{
    // Хранилище загруженных плагинов
    private static readonly List<IVehiclePlugin> _vehiclePlugins = new();

    // Публичный доступ к плагинам только для чтения
    public static IReadOnlyList<IVehiclePlugin> VehiclePlugins => _vehiclePlugins;

    // Загрузка всех плагинов из папки
    public static void LoadFromFolder(string folderPath)
    {
        // Создаём папку, если её нет
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            return;
        }

        // Перебираем все DLL в папке
        foreach (string dllPath in Directory.GetFiles(folderPath, "*.dll"))
        {
            // Пропускаем VehicleCore.dll — она уже в основном приложении
            if (Path.GetFileName(dllPath).Equals("VehicleCore.dll", StringComparison.OrdinalIgnoreCase))
                continue;

            // Загружаем плагин
            LoadPluginAssembly(dllPath);
        }
    }

    // Загрузка одного плагина из файла
    public static void LoadFromFile(string dllPath)
    {
        // Проверяем существование файла
        if (!File.Exists(dllPath))
        {
            Console.WriteLine("Файл плагина не найден.");
            return;
        }

        LoadPluginAssembly(dllPath);
    }

    // Основная логика загрузки сборки плагина
    private static void LoadPluginAssembly(string dllPath)
    {
        try
        {
            // Создаём изолированный контекст для плагина (свои зависимости)
            var context = new PluginAssemblyLoadContext(dllPath);

            // Загружаем сборку в память
            Assembly assembly = context.LoadFromAssemblyPath(Path.GetFullPath(dllPath));

            // Перебираем все публичные типы в сборке
            foreach (Type type in assembly.GetExportedTypes())
            {
                // Фильтруем: только классы, реализующие IVehiclePlugin, не абстрактные, не интерфейсы
                if (typeof(IVehiclePlugin).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    // Создаём экземпляр класса и проверяем, что он реализует IVehiclePlugin
                    if (Activator.CreateInstance(type) is IVehiclePlugin plugin)
                    {
                        // Проверяем, нет ли уже плагина с таким же типом транспорта
                        if (_vehiclePlugins.Any(p => p.VehicleType == plugin.VehicleType))
                            continue;

                        // Добавляем плагин в коллекцию
                        _vehiclePlugins.Add(plugin);
                        Console.WriteLine($"Загружен плагин: {plugin.VehicleTypeName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки плагина {dllPath}: {ex.Message}");
        }
    }

    // Изолированный контекст загрузки для каждого плагина
    private sealed class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginAssemblyLoadContext(string pluginPath) : base(isCollectible: false)
        {
            // Резолвер для поиска зависимостей плагина
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        // Переопределяем загрузку зависимостей
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            // Если требуется VehicleCore — отдаём уже загруженную версию из основного приложения
            if (assemblyName.Name == "VehicleCore")
                return typeof(Vehicle).Assembly;

            // Ищем зависимость в папке плагина
            string? dependencyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (dependencyPath != null)
                return LoadFromAssemblyPath(dependencyPath);

            // Зависимость не найдена
            return null;
        }
    }
}