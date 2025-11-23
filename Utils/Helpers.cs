using Microsoft.Xna.Framework;
using Terraria;
using TShockAPI;

namespace MimicSpawner;

public class Helpers
{
    public static readonly Random random = new();

    public static short GetChestIndex(TSPlayer player)
    {
        for (short i = 0; i < 50; i++)
        {
            Item item = player.TPlayer.inventory[i];
            if (item.Name.EndsWith("Chest") && item.stack > 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static bool SpawnMimic(TSPlayer player, short type)
    {
        TShock.Utils.GetRandomClearTileWithInRange(
            player.TileX,
            player.TileY,
            20,
            10,
            out int tileX,
            out int tileY
        );
        int npcIndex = NPC.NewNPC(null, tileX * 16, tileY * 16, type);
        if (npcIndex != 200)
        {
            player.SendSuccessMessage($"You summoned a {TShock.Utils.GetNPCById(type).FullName}!");
            return true;
        }

        player.SendErrorMessage("Failed to summon a Mimic. The NPC cap might have been reached.");
        return false;
    }
}
