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
}
