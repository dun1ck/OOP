namespace ConsoleApp1.Commands;

/// <summary>
/// Паттерн Command: инкапсулирует действие меню.
/// </summary>
public interface IMenuCommand
{
    string Key { get; }
    string Title { get; }
    void Execute();
}
