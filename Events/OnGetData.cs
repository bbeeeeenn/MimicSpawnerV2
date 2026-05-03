using Terraria;
using Terraria.ID;
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

    private void EventHandler(GetDataEventArgs args)
    {
        if (
            !PluginSettings.Config.Enabled
            || args.MsgID != PacketTypes.PlayerUpdate
            || !Main.hardMode
        )
        {
            return;
        }

        using BinaryReader reader = new(
            new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)
        );
        var playerId = reader.ReadByte();
        BitsByte control = reader.ReadByte();
        _ = reader.ReadByte();
        _ = reader.ReadByte();
        _ = reader.ReadByte();
        var selectedItemIndex = reader.ReadByte();

        if (!control[5]) // if not useitem
        {
            return;
        }

        TSPlayer player = TShock.Players[playerId];
        Item selectedItem = player.TPlayer.inventory[selectedItemIndex];

        if (
            !new List<int>() { ItemID.NightKey, ItemID.LightKey, ItemID.GoldenKey }.Contains(
                selectedItem.type
            )
            || (
                (DateTime.Now - player.GetData<DateTime>("lastSummon")).TotalMilliseconds
                < PluginSettings.Config.CooldownInSeconds * 1000
            )
        )
        {
            return;
        }

        short mimicType = -1; // netId
        short chestIndex = -1;
        if (PluginSettings.Config.RequireChest)
        {
            chestIndex = Helpers.GetChestIndex(player);
            if (chestIndex == -1)
            {
                player.SendErrorMessage(
                    "You must have at least one chest of any type in your inventory to summon a Mimic."
                );
                return;
            }
        }

        if (PluginSettings.Config.WeakerMimics && selectedItem.type == ItemID.GoldenKey)
        // Regular Mimic
        {
            mimicType = player.TPlayer.ZoneSnow ? NPCID.IceMimic : NPCID.Mimic;
        }
        else if (selectedItem.type == ItemID.NightKey)
        // Big Evil Mimic
        {
            short[] evilMimicType = { NPCID.BigMimicCorruption, NPCID.BigMimicCrimson };
            string mode = PluginSettings.Config.Mode.ToLower();
            mimicType =
                mode == "biome"
                    ? (
                        player.TPlayer.ZoneCorrupt ? evilMimicType[0]
                        : player.TPlayer.ZoneCrimson ? evilMimicType[1]
                        : (short)-1
                    )
                : mode == "random" ? evilMimicType[Helpers.random.Next(2)]
                : (WorldGen.crimson ? evilMimicType[1] : evilMimicType[0]); // default
            if (mode == "biome" && mimicType == -1)
            {
                player.SendErrorMessage("You have to be in an Evil biome to use this.");
            }
        }
        else if (selectedItem.type == ItemID.LightKey)
        // Big Hallow Mimic
        {
            mimicType = NPCID.BigMimicHallow;
        }

        if (mimicType == -1)
        {
            return;
        }

        // Spawn mimic
        bool success = Helpers.SpawnMimic(player, mimicType);
        if (success)
        {
            player.SetData("lastSummon", DateTime.Now);
            selectedItem.stack--;
            player.SendData(PacketTypes.PlayerSlot, "", player.Index, selectedItemIndex);
            if (chestIndex != -1)
            {
                player.TPlayer.inventory[chestIndex].stack--;
                player.SendData(PacketTypes.PlayerSlot, "", player.Index, chestIndex);
            }
        }
    }
}
