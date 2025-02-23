using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace TShockPlugin
{
    [ApiVersion(2, 1)]
    public class TShockPlugin : TerrariaPlugin
    {
        public static readonly string PluginName = "TShockPluginTemplate"; // Configure this
        public override string Name => PluginName;
        public override string Author => "TRANQUILZOIIP - github.com/bbeeeeenn";
        public override string Description => base.Description;
        public override Version Version => base.Version;

        public TShockPlugin(Main game)
            : base(game) { }

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
