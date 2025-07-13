using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Entities;
using Wrecept.Core;
using Wrecept.Storage.Services;
using Xunit;

namespace Wrecept.Storage.Tests;

public class SettingsSessionLogServiceTests
{
    [Fact]
    public async Task SettingsService_LoadsDefaults_WhenFileMissing()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var svc = new SettingsService(path);

        var settings = await svc.LoadAsync();

        Assert.NotNull(settings);
        Assert.Equal(string.Empty, settings.DatabasePath);
        Assert.Equal(string.Empty, settings.UserInfoPath);
    }

    [Fact]
    public async Task SettingsService_SaveAndLoad_RoundTrip()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var svc = new SettingsService(path);
        var original = new AppSettings
        {
            DatabasePath = "db",
            UserInfoPath = "info",
            ScreenMode = ScreenMode.Small
        };

        await svc.SaveAsync(original);
        var loaded = await svc.LoadAsync();

        Assert.Equal(original.DatabasePath, loaded.DatabasePath);
        Assert.Equal(original.UserInfoPath, loaded.UserInfoPath);
        Assert.Equal(original.ScreenMode, loaded.ScreenMode);
    }

    [Fact]
    public async Task SessionService_ReturnsNull_WhenFileMissing()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var svc = new SessionService(path);

        var id = await svc.LoadLastInvoiceIdAsync();

        Assert.Null(id);
    }

    [Fact]
    public async Task SessionService_SaveAndLoad_RoundTrip()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var svc = new SessionService(path);

        await svc.SaveLastInvoiceIdAsync(12);
        var loaded = await svc.LoadLastInvoiceIdAsync();

        Assert.Equal(12, loaded);
    }

    [Fact]
    public async Task SessionService_Delete_WhenNullPassed()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var svc = new SessionService(path);

        await svc.SaveLastInvoiceIdAsync(5);
        await svc.SaveLastInvoiceIdAsync(null);

        Assert.False(File.Exists(path));
    }

    [Fact]
    public async Task LogService_WritesFile()
    {
        var tempHome = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempHome);
        var oldAppData = Environment.GetEnvironmentVariable("APPDATA");
        Environment.SetEnvironmentVariable("APPDATA", tempHome);

        try
        {
            var svc = new SerilogLogService();
            await svc.LogError("test", new InvalidOperationException());

            var logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Wrecept", "logs");
            Assert.True(Directory.Exists(logDir));
            var files = Directory.GetFiles(logDir);
            Assert.NotEmpty(files);
            var content = await File.ReadAllTextAsync(files[0]);
            Assert.Contains("test", content);
        }
        finally
        {
            Environment.SetEnvironmentVariable("APPDATA", oldAppData);
            if (Directory.Exists(tempHome))
            {
                Directory.Delete(tempHome, recursive: true);
            }
        }
    }
}
