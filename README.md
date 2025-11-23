# MimicSpawner Plugin

A plugin that allows players to instantly spawn a Mimic.

## How to Use

-  Hold a **Key of Night** or **Key of Light**, depending on which type of Mimic you want to spawn.
-  Use the key as if it were a consumable item to summon the Mimic instantly.
-  Note: Evil Mimics has several spawn modes; see details below.

## Configuration

-  **`Enabled`** (Default: `true`) — Enable or disable the plugin.
-  **`RequireChest`** (Default: `false`) — If `true`, players must have at least one chest in their inventory to summon a Mimic.
-  **`CooldownInSeconds`** (Default: `1`) — Cooldown (in seconds) before a player can spawn another Mimic.
-  **`Mode`** (Default: `"biome"`) — Determines how the type of Evil Mimic is chosen. Available modes:
   -  `default`: Uses the evil type the world can naturally spawn (Corrupt or Crimson). Can be summoned anywhere.
   -  `biome`: When using a Key of Night, the player must be in an evil biome; the spawned Evil Mimic will match that biome.
   -  `random`: Randomly chooses between the two Evil Mimic types (Corrupt or Crimson). Can be summoned anywhere.
-  **`WeakerMimics`** (Default: `true`) — If `true`, using a Golden Key may summon a weaker Mimic (Regular or Ice Mimic when in a Snow biome).

## Installation

1. [Download the plugin `.dll` file from the Releases page](https://github.com/bbeeeeenn/MimicSpawnerV2/releases/).
2. Place the file in your TShock server’s `ServerPlugins` folder.
3. Restart your TShock server.
4. The plugin should now be active and ready to use.
