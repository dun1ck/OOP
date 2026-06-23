namespace ConsoleApp1;

public sealed class AppSettings
{
    private static readonly Lazy<AppSettings> _instance = new(() => new AppSettings());
    public static AppSettings Instance => _instance.Value;

    private readonly HashSet<string> _enabledProcessingPlugins = new(StringComparer.OrdinalIgnoreCase);

    private AppSettings() { }

    public IReadOnlyCollection<string> EnabledProcessingPlugins => _enabledProcessingPlugins;

    public bool IsEnabled(string pluginName) => _enabledProcessingPlugins.Contains(pluginName);

    public void SetEnabled(string pluginName, bool enabled)
    {
        if (enabled)
            _enabledProcessingPlugins.Add(pluginName);
        else
            _enabledProcessingPlugins.Remove(pluginName);
    }
}
