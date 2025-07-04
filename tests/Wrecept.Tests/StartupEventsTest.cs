using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Wrecept.Wpf;
using Wrecept.Core.Entities;
using Xunit;

namespace Wrecept.Tests;

public class StartupEventsTest
{
    [Fact]
    public async Task EnsureServicesInitializedAsync_Idempotent()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dataDir = Path.Combine(appData, "Wrecept");
        Directory.CreateDirectory(dataDir);
        var settingsPath = Path.Combine(dataDir, "settings.json");
        var settings = new AppSettings
        {
            DatabasePath = Path.Combine(dataDir, "test.db"),
            UserInfoPath = Path.Combine(dataDir, "user.json")
        };
        await File.WriteAllTextAsync(settingsPath, JsonSerializer.Serialize(settings));

        var method = typeof(App).GetMethod("EnsureServicesInitializedAsync", BindingFlags.NonPublic | BindingFlags.Static)!;
        await (Task)method.Invoke(null, null)!;
        var first = App.Provider;

        await (Task)method.Invoke(null, null)!;
        var second = App.Provider;

        Assert.Same(first, second);
    }
}

