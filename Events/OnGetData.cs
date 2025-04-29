using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace MimicSpawner.Events;

public class OnGetData : Models.Event
{
    public override void Disable(TerrariaPlugin plugin)
    {
        ServerApi.Hooks.NetGetData.Deregister(plugin, EventHandler);
    }

    public override void Enable(TerrariaPlugin plugin)
    {
        ServerApi.Hooks.NetGetData.Register(plugin, EventHandler);
    }

    private static readonly Random random = new();

    private void EventHandler(GetDataEventArgs args)
    {
        if (args.MsgID != PacketTypes.PlayerUpdate)
            return;

        using BinaryReader reader = new(
            new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)
        );
        var playerId = reader.ReadByte();
        BitsByte control = reader.ReadByte();
        _ = reader.ReadByte();
        _ = reader.ReadByte();
        _ = reader.ReadByte();
        var selectedSlot = reader.ReadByte();

        bool useItem = control[5];
        if (!useItem)
            return;

        TSPlayer player = TShock.Players[playerId];
        Item selectedItem = player.TPlayer.inventory[selectedSlot];

        if (
            !useItem
            || !new List<int>()
            {
                Terraria.ID.ItemID.NightKey,
                Terraria.ID.ItemID.LightKey,
            }.Contains(selectedItem.netID)
            || !(
                !TShockPlugin.LastSummon.ContainsKey(player.Name)
                || (DateTime.Now - TShockPlugin.LastSummon[player.Name]).Seconds
                    >= PluginSettings.Config.CooldownInSeconds
            )
        )
            return;

        int? chestIndex = null;
        Item? chest = null;
        if (PluginSettings.Config.RequireChest)
        {
            chestIndex = GetChestIndex(player);
            if (chestIndex == null)
            {
                player.SendErrorMessage(
                    "You must have at least one chest of any type in your inventory to summon a Mimic."
                );
                return;
            }

            chest = player.TPlayer.inventory[(int)chestIndex];
        }

        if (!SpawnMimic(player, selectedItem.netID == Terraria.ID.ItemID.NightKey))
            return;

        // Consume key
        TShockPlugin.LastSummon[player.Name] = DateTime.Now;
        selectedItem.stack--;
        NetMessage.SendData(
            (int)PacketTypes.PlayerSlot,
            -1,
            -1,
            null,
            playerId,
            selectedSlot,
            selectedItem.stack,
            selectedItem.prefix,
            selectedItem.netID
        );

        // Consume chest if Config.RequireChest
        if (chest != null && chestIndex != null)
        {
            chest.stack--;
            NetMessage.SendData(
                (int)PacketTypes.PlayerSlot,
                -1,
                -1,
                null,
                playerId,
                (float)chestIndex,
                chest.stack,
                chest.prefix,
                chest.netID
            );
        }
    }

    private static int? GetChestIndex(TSPlayer player)
    {
        for (int i = 0; i < NetItem.InventorySlots; i++)
        {
            Item item = player.TPlayer.inventory[i];
            if (item.Name.EndsWith("Chest"))
            {
                return i;
            }
        }
        return null;
    }

    private static bool SpawnMimic(TSPlayer player, bool nightKey)
    {
        Vector2 playerPosition = player.TPlayer.position;
        int offset = random.Next(-400, 400);
        Vector2 position = new(playerPosition.X + offset, playerPosition.Y);
        List<short> evilMimics = new()
        {
            Terraria.ID.NPCID.BigMimicCorruption,
            Terraria.ID.NPCID.BigMimicCrimson,
        };
        int type = nightKey
            ? evilMimics[random.Next(evilMimics.Count)]
            : Terraria.ID.NPCID.BigMimicHallow;
        int npcIndex = NPC.NewNPC(null, (int)position.X, (int)position.Y, type);
        if (npcIndex != 200)
        {
            player.SendSuccessMessage($"You summoned a {TShock.Utils.GetNPCById(type).FullName}!");
            return true;
        }
        else
        {
            player.SendErrorMessage("Can't summon a Mimic. The NPC cap might have been reached.");
            return false;
        }
    }
}
