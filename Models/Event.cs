using TerrariaApi.Server;

namespace MimicSpawner.Models;

public abstract class Event
{
    public abstract void Enable(TerrariaPlugin plugin);
    public abstract void Disable(TerrariaPlugin plugin);
}
