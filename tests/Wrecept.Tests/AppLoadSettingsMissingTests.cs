using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Wrecept.Wpf;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class AppLoadSettingsMissingTests
{
    private static async Task<AppSettings> InvokeLoadAsync(INotificationService? n = null, ISetupFlow? f = null)
    {
        var m = typeof(App).GetMethod("LoadSettingsAsync", BindingFlags.NonPublic | BindingFlags.Static)!;
        return await (Task<AppSettings>)m.Invoke(null, new object?[] { n, f })!;
    }

    [StaFact]
    public async Task LoadSettingsAsync_CreatesDefaultWhenMissing()
    {
        var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable("APPDATA", temp);
        var flow = new DummyFlow();
        var notif = new DummyNotif();
        try
        {
            var settings = await InvokeLoadAsync(notif, flow);
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

    private class DummyFlow : ISetupFlow
    {
        public Task<SetupData> RunAsync(string db, string cfg)
            => Task.FromResult(new SetupData(db, cfg, new UserInfo()));
    }

    private class DummyNotif : INotificationService
    {
        public void ShowError(string message) { }
        public void ShowInfo(string message) { }
        public bool Confirm(string message) => true;
    }
}
