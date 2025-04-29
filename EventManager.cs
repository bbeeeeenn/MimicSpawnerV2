using MimicSpawner.Events;
using MimicSpawner.Models;
using TerrariaApi.Server;

namespace MimicSpawner;

public class EventManager
{
    public static readonly List<Event> events = new()
    {
        // Events
        new OnReload(),
    };

    public static void RegisterAll(TerrariaPlugin plugin)
    {
        foreach (Event _event in events)
        {
            _event.Enable(plugin);
        }
    }

    public static void DeregisterAll(TerrariaPlugin plugin)
    {
        foreach (Event _event in events)
        {
            _event.Disable(plugin);
        }
    }
}
