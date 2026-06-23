namespace ConsoleApp1.Commands;

public class MenuCommandRegistry
{
    private readonly Dictionary<string, IMenuCommand> _commands = new();

    public void Register(IMenuCommand command) => _commands[command.Key] = command;

    public bool TryExecute(string? key)
    {
        if (key != null && _commands.TryGetValue(key, out IMenuCommand? cmd))
        {
            cmd.Execute();
            return true;
        }
        return false;
    }

    public IEnumerable<IMenuCommand> All => _commands.Values.OrderBy(c => c.Key);
}
