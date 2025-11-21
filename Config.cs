using Microsoft.Xna.Framework;
using MimicSpawner.Models;
using Newtonsoft.Json;
using TShockAPI;

namespace MimicSpawner;

public class PluginSettings
{
    public static readonly string PluginDirectory = Path.Combine(
        TShock.SavePath,
        TShockPlugin.PluginName
    );
    public static readonly string ConfigPath = Path.Combine(PluginDirectory, $"config.json");

    public static PluginSettings Config { get; set; } = new();

    #region Configs
    public bool Enabled = true;
    public bool RequireChest = false;
    public int CooldownInSeconds = 1;
    public string Mode = "biome";
    public string[] Help =
    {
        "You use [Key of Night] to summon an Evil Mimic, and [Key of Light] to summon a Hallow Mimic",
        "Available modes: 'default', 'biome', 'random'",
        "default: Follows the type of evil mimic the world would normally spawn. Recommended for the 'getfixedboi' seed, where the Evil Mimic type changes each day. Can summon anywhere.",
        "biome: When using a [Night Key], you must be in an evil biome. The type of Evil Mimic that appears will match the biome.",
        "random: Randomly chooses between the two Evil Mimic types. Can summon anywhere.",
        "[CAUTION]: 'mode' is not case-sensitive. If an invalid value is entered, it will automatically set to 'default'.",
    };
    public bool WeakerMimics = true;
    public string[] Help2 =
    {
        "You use [Golden Key] to summon a Mimic",
        "The summoned mimic will be an [Ice Mimic] if you're in a [Snow biome].",
    };
    #endregion


    public static void Save()
    {
        string configJson = JsonConvert.SerializeObject(Config, Formatting.Indented);
        File.WriteAllText(ConfigPath, configJson);
    }

    public static ResponseMessage Load()
    {
        if (!Directory.Exists(PluginDirectory))
        {
            Directory.CreateDirectory(PluginDirectory);
        }
        if (!File.Exists(ConfigPath))
        {
            Save();
            return new ResponseMessage()
            {
                Text =
                    $"[{TShockPlugin.PluginName}] Config file doesn't exist yet. A new one has been created.",
                Color = Color.Yellow,
            };
        }
        else
        {
            try
            {
                string json = File.ReadAllText(ConfigPath);
                PluginSettings? deserializedConfig = JsonConvert.DeserializeObject<PluginSettings>(
                    json
                );
                if (deserializedConfig != null)
                {
                    Config = deserializedConfig;

                    // Set to default if 'mode' is invalid
                    if (
                        !new List<string>() { "default", "biome", "random" }.Contains(
                            Config.Mode.ToLower()
                        )
                    )
                    {
                        Config.Mode = "default";
                        Save();
                    }
                    if (Config.CooldownInSeconds < 1)
                    {
                        Config.CooldownInSeconds = 1;
                        Save();
                    }

                    return new ResponseMessage()
                    {
                        Text = $"[{TShockPlugin.PluginName}] Loaded config.",
                        Color = Color.LimeGreen,
                    };
                }
                else
                {
                    return new ResponseMessage()
                    {
                        Text =
                            $"[{TShockPlugin.PluginName}] Config file was found, but deserialization returned null.",
                        Color = Color.Red,
                    };
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError(
                    $"[{TShockPlugin.PluginName}] Error loading config: {ex.Message}"
                );
                return new ResponseMessage()
                {
                    Text =
                        $"[{TShockPlugin.PluginName}] Error loading config. Check logs for more details.",
                    Color = Color.Red,
                };
            }
        }
    }
}
