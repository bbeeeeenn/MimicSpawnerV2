using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;
using TShockPlugin.Models;
using TShockPlugin.Utils;

namespace TShockPlugin.Events;

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
        MessageResponse response = PluginSettings.Load();
        player.SendMessage(response.Text, response.Color);
    }
}
