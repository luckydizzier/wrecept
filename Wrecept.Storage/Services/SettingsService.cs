using System.Text.Json;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;

namespace Wrecept.Storage.Services;

public class SettingsService : ISettingsService
{
    private readonly string _path;

    public SettingsService(string path)
    {
        _path = path;
    }

    public async Task<AppSettings> LoadAsync()
    {
        if (!File.Exists(_path)) return new AppSettings();
        using var stream = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<AppSettings>(stream) ?? new AppSettings();
    }

    public async Task SaveAsync(AppSettings settings)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        using var stream = File.Create(_path);
        await JsonSerializer.SerializeAsync(stream, settings, new JsonSerializerOptions { WriteIndented = true });
    }
}
