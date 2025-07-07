using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Wrecept.Wpf;
using Wrecept.Core.Entities;
using Xunit;

namespace Wrecept.Tests;

public class AppLoadSettingsMissingTests
{
    private static async Task<AppSettings> InvokeLoadAsync()
    {
        var m = typeof(App).GetMethod("LoadSettingsAsync", BindingFlags.NonPublic | BindingFlags.Static)!;
        return await (Task<AppSettings>)m.Invoke(null, null)!;
    }

    [StaFact(Skip="Requires UI interaction")]
    public async Task LoadSettingsAsync_CreatesDefaultWhenMissing()
    {
        var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable("APPDATA", temp);
        try
        {
            var settings = await InvokeLoadAsync();
            Assert.NotEqual(string.Empty, settings.DatabasePath);
            Assert.NotEqual(string.Empty, settings.UserInfoPath);
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
