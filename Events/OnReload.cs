using MimicSpawner.Models;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace MimicSpawner.Events;

public class OnReload : Event
{
    public override void Disable(TerrariaPlugin plugin)
    {
        GeneralHooks.ReloadEvent -= EventMethod;
    }

    public override void Enable(TerrariaPlugin plugin)
    {
        GeneralHooks.ReloadEvent += EventMethod;
    }

    private void EventMethod(ReloadEventArgs e)
    {
        TSPlayer player = e.Player;
        ResponseMessage response = PluginSettings.Load();
        player.SendMessage(response.Text, response.Color);
    }
}
