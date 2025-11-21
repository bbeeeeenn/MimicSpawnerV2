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
        Vector2 playerPosition = player.TPlayer.position;
        int offset = random.Next(-400, 400);
        Vector2 position = new(playerPosition.X + offset, playerPosition.Y);

        int npcIndex = NPC.NewNPC(null, (int)position.X, (int)position.Y, type);
        if (npcIndex != 200)
        {
            player.SendSuccessMessage($"You summoned a {TShock.Utils.GetNPCById(type).FullName}!");
            return true;
        }

        player.SendErrorMessage("Failed to summon a Mimic. The NPC cap might have been reached.");
        return false;
    }
}
