using Microsoft.Xna.Framework;
using TShockAPI;

namespace TShockPlugin.Commands;

public class DummyCommand : Models.Command
{
    public override bool AllowServer => base.AllowServer;
    public override string[] Aliases { get; set; } =
        { "templatecommand", "dummycommand", "testcommand" };
    public override string PermissionNode { get; set; } = "template.command";

    public override void Execute(CommandArgs args)
    {
        TSPlayer player = args.Player;
        player.SendMessage($"Hi {player.Name}!", Color.Blue);
    }
}
