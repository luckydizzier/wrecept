using System.Text.Json;
using Wrecept.Core.Models;
using System.IO;

namespace Wrecept.Core.Services;

public class SettingsService : ISettingsService
{
    private readonly string _settingsPath;
    private ApplicationSettings _currentSettings = new();
    public event EventHandler<ApplicationSettings>? SettingsChanged;

    public SettingsService(string? settingsPath = null)
    {
        _settingsPath = settingsPath ?? Path.Combine(AppContext.BaseDirectory, "wrecept.json");
    }

    public async Task<ApplicationSettings> LoadAsync()
    {
        if (!File.Exists(_settingsPath))
        {
            _currentSettings = new ApplicationSettings();
            await SaveAsync(_currentSettings);
            return _currentSettings;
        }

        try
        {
            await using var stream = File.OpenRead(_settingsPath);
            var settings = await JsonSerializer.DeserializeAsync<ApplicationSettings>(stream);
            _currentSettings = settings ?? new ApplicationSettings();
        }
        catch (JsonException)
        {
            _currentSettings = new ApplicationSettings();
            await SaveAsync(_currentSettings);
        }

        return _currentSettings;
    }

    public async Task SaveAsync(ApplicationSettings settings)
    {
        _currentSettings = settings;
        var json = JsonSerializer.Serialize(_currentSettings, new JsonSerializerOptions { WriteIndented = true });
        var directory = Path.GetDirectoryName(_settingsPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        await File.WriteAllTextAsync(_settingsPath, json);
        SettingsChanged?.Invoke(this, _currentSettings);
    }

    public async Task UpdateThemeAsync(string theme)
    {
        _currentSettings.Theme = theme;
        await SaveAsync(_currentSettings);
    }

    public async Task UpdateLanguageAsync(string language)
    {
        _currentSettings.Language = language;
        await SaveAsync(_currentSettings);
    }
}
