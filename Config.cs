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
    public bool RequireChest = true;
    public int CooldownInSeconds = 2;
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
