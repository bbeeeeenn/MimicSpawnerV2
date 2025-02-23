using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using TShockAPI;
using TShockPlugin.Models;

namespace TShockPlugin;

public class PluginSettings
{
    public static string PluginDisplayName { get; set; } = "Plugin";
    public static readonly string ConfigPath = Path.Combine(
        TShock.SavePath,
        $"{PluginDisplayName}.json"
    );
    public static PluginSettings Config { get; set; } = new();

    #region Configs

    #endregion
    public static void Save()
    {
        string configJson = JsonConvert.SerializeObject(Config, Formatting.Indented);
        File.WriteAllText(ConfigPath, configJson);
    }

    public static ResponseMessage Load()
    {
        if (File.Exists(ConfigPath))
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
                        Text = $"[{PluginDisplayName}] Loaded config.",
                        Color = Color.LimeGreen,
                    };
                }
                else
                {
                    return new ResponseMessage()
                    {
                        Text =
                            $"[{PluginDisplayName}] Config file was found, but deserialization returned null.",
                        Color = Color.Red,
                    };
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError(
                    $"[{PluginDisplayName}] Error loading config: {ex.Message}"
                );
                return new ResponseMessage()
                {
                    Text =
                        $"[{PluginDisplayName}] Error loading config. Check logs for more details.",
                    Color = Color.Red,
                };
            }
        }
        else
        {
            Save();
            return new ResponseMessage()
            {
                Text =
                    $"[{PluginDisplayName}] Config file doesn't exist yet. A new one has been created.",
                Color = Color.Yellow,
            };
        }
    }
}
