using System;
using System.IO;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Core.Tests;

public class SettingsServiceTests
{
    [Fact]
    public async Task SaveAndLoad_RoundTrip_Works()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
        var service = new SettingsService(tempFile);
        var settings = new ApplicationSettings { DatabasePath = "test.db", Theme = "Dark", Language = "hu" };
        await service.SaveAsync(settings);
        var loaded = await service.LoadAsync();
        Assert.Equal(settings.DatabasePath, loaded.DatabasePath);
        Assert.Equal(settings.Theme, loaded.Theme);
        Assert.Equal(settings.Language, loaded.Language);
        File.Delete(tempFile);
    }

    [Fact]
    public async Task UpdateThemeAsync_Persists_And_RaisesEvent()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
        var service = new SettingsService(tempFile);
        await service.LoadAsync();
        string? changedTheme = null;
        service.SettingsChanged += (_, s) => changedTheme = s.Theme;

        await service.UpdateThemeAsync("Dark");

        Assert.Equal("Dark", changedTheme);
        var reloaded = await service.LoadAsync();
        Assert.Equal("Dark", reloaded.Theme);
        File.Delete(tempFile);
    }

    [Fact]
    public async Task LoadAsync_InvalidJson_ReturnsDefaults()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
        await File.WriteAllTextAsync(tempFile, "{ invalid json }");
        var service = new SettingsService(tempFile);
        var loaded = await service.LoadAsync();
        Assert.Equal("Data/wrecept.db", loaded.DatabasePath);
        File.Delete(tempFile);
    }

    [Fact]
    public async Task SaveAsync_CreatesDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var tempFile = Path.Combine(dir, "settings.json");
        var service = new SettingsService(tempFile);
        await service.SaveAsync(new ApplicationSettings());
        Assert.True(File.Exists(tempFile));
        Directory.Delete(dir, true);
    }
}
