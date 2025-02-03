using Microsoft.Xna.Framework;
using TerrariaApi.Server;
using TShockAPI;

namespace TShockPlugin.Events;

public class OnGameInitialize : Models.Event
{
    public override void Disable(TerrariaPlugin plugin)
    {
        ServerApi.Hooks.GameInitialize.Deregister(plugin, EventMethod);
    }

    public override void Enable(TerrariaPlugin plugin)
    {
        ServerApi.Hooks.GameInitialize.Register(plugin, EventMethod);
    }

    private void EventMethod(EventArgs args)
    {
        TShock.Utils.Broadcast("Hello World!", Color.White);
    }
}
