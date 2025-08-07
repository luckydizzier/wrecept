using System.Text.Json;
using Wrecept.Core.Models;
using System.IO;

namespace Wrecept.Core.Services;

public class SettingsService : ISettingsService
{
    private readonly string _settingsPath;

    public SettingsService(string? settingsPath = null)
    {
        _settingsPath = settingsPath ?? Path.Combine(AppContext.BaseDirectory, "wrecept.json");
    }

    public async Task<ApplicationSettings> LoadAsync()
    {
        if (!File.Exists(_settingsPath))
        {
            var defaults = new ApplicationSettings();
            await SaveAsync(defaults);
            return defaults;
        }

        await using var stream = File.OpenRead(_settingsPath);
        var settings = await JsonSerializer.DeserializeAsync<ApplicationSettings>(stream);
        return settings ?? new ApplicationSettings();
    }

    public async Task SaveAsync(ApplicationSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_settingsPath, json);
    }
}
