using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf;
using Wrecept.Core.Entities;

namespace Wrecept.Tests;

public class AppStartupTests
{
    private static async Task<AppSettings> InvokeLoadAsync(INotificationService? n = null, ISetupFlow? f = null)
    {
        var method = typeof(App).GetMethod("LoadSettingsAsync", BindingFlags.NonPublic | BindingFlags.Static)!;
        return await (Task<AppSettings>)method.Invoke(null, new object?[] { n, f })!;
    }

    [StaFact]
    public async Task LoadSettingsAsync_Returns_Settings_FromJson()
    {
        var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable("APPDATA", temp);
        Directory.CreateDirectory(Path.Combine(temp, "Wrecept"));
        var file = Path.Combine(temp, "Wrecept", "settings.json");
        var expected = new AppSettings { DatabasePath = "db", UserInfoPath = "user" };
        await File.WriteAllTextAsync(file, JsonSerializer.Serialize(expected));

        try
        {
            var settings = await InvokeLoadAsync();
            Assert.Equal(expected.DatabasePath, settings.DatabasePath);
            Assert.Equal(expected.UserInfoPath, settings.UserInfoPath);
        }
        finally
        {
            Directory.Delete(temp, true);
            Environment.SetEnvironmentVariable("APPDATA", null);
        }
    }

    [StaFact]
    public async Task LoadSettingsAsync_Returns_Default_OnInvalidJson()
    {
        var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable("APPDATA", temp);
        Directory.CreateDirectory(Path.Combine(temp, "Wrecept"));
        var file = Path.Combine(temp, "Wrecept", "settings.json");
        await File.WriteAllTextAsync(file, "{ invalid }");

        try
        {
            var settings = await InvokeLoadAsync();
            Assert.Equal(string.Empty, settings.DatabasePath);
            Assert.Equal(string.Empty, settings.UserInfoPath);
            var logDir = Path.Combine(temp, "Wrecept", "logs");
            Assert.True(Directory.Exists(logDir));
            Assert.NotEmpty(Directory.GetFiles(logDir));
        }
        finally
        {
            Directory.Delete(temp, true);
            Environment.SetEnvironmentVariable("APPDATA", null);
        }
    }
}
