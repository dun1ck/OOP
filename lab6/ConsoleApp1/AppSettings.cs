namespace ConsoleApp1;

/// <summary>
/// Паттерн Singleton: единые настройки приложения.
/// </summary>
public sealed class AppSettings
{
    private static readonly Lazy<AppSettings> _instance = new(() => new AppSettings());
    public static AppSettings Instance => _instance.Value;

    private readonly HashSet<string> _enabled = new(StringComparer.OrdinalIgnoreCase);
    private AppSettings() { }

    public bool IsEnabled(string name) => _enabled.Contains(name);

    public void SetEnabled(string name, bool enabled)
    {
        if (enabled) _enabled.Add(name);
        else _enabled.Remove(name);
    }
}
