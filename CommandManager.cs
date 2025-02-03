using TShockPlugin.Commands;
using TShockPlugin.Models;

namespace TShockPlugin;

public class CommandManager
{
    public static readonly List<Command> Commands = new()
    {
        // Commands
        new DummyCommand(),
    };

    public static void RegisterAll()
    {
        foreach (Command command in Commands)
        {
            TShockAPI.Commands.ChatCommands.Add(command);
        }
    }
}
