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
}
