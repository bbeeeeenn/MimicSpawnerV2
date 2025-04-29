using MimicSpawner.Commands;
using MimicSpawner.Models;

namespace MimicSpawner;

public class CommandManager
{
    public static readonly List<Command> Commands = new()
    {
        // Commands
        // new DummyCommand(),
    };

    public static void RegisterAll()
    {
        foreach (Command command in Commands)
        {
            TShockAPI.Commands.ChatCommands.Add(command);
        }
    }
}
