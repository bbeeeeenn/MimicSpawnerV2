using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockPlugin.Utils;

namespace TShockPlugin
{
    [ApiVersion(2, 1)]
    public class TShockPlugin : TerrariaPlugin
    {
        public override string Name => "TShock Plugin";
        public override string Author => "TRANQUILZOIIP - github.com/bbeeeeenn";
        public override string Description => base.Description;
        public override Version Version => base.Version;

        public TShockPlugin(Main game)
            : base(game)
        {
            PluginSettings.PluginDisplayName = Name;
        }

        public override void Initialize()
        {
            // Load config
            TShock.Log.ConsoleInfo(PluginSettings.Load().Text);
            // Load events
            EventManager.RegisterAll(this);
            // Load commands
            CommandManager.RegisterAll();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EventManager.DeregisterAll(this);
            }
            base.Dispose(disposing);
        }
    }
}
