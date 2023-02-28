using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UI.Model;

namespace UI.Services;

internal class SettingsService
{
    private const string settingsPath = "settings.json";
    private static readonly object readLock = new();
    private static readonly JsonSerializerOptions jsonOptions;

    static SettingsService()
    {
        jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            
        };
        jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public static void Save(Settings settings)
    {
        using var fs = new FileStream(settingsPath, FileMode.Create);
       
        JsonSerializer.Serialize(fs, settings, jsonOptions);
    }

    public static Settings Read()
    {
        if (!File.Exists(settingsPath))
            return new Settings();
        lock (readLock)
        {
            using var fs = new FileStream(settingsPath, FileMode.Open);
            return JsonSerializer.Deserialize<Settings>(fs, jsonOptions);
        }
    }
}