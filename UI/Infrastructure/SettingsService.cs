using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using UI.Model;

namespace UI.Infrastructure;

internal class SettingsService
{
    public static async Task Save(Settings settings)
    {
        await using var fs = new FileStream("settings.json", FileMode.Create);
        await JsonSerializer.SerializeAsync(fs, settings);
    }

    public static async Task<Settings> Read()
    {
        if(!File.Exists("settings.json"))
            return new Settings();
        await using var fs = new FileStream("settings.json", FileMode.OpenOrCreate);
        return await JsonSerializer.DeserializeAsync<Settings>(fs);
    }
}