using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;
using Wrecept.Wpf;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class AppStartupEventsTests
{
    private static async Task<AppSettings> InvokeLoadAsync(INotificationService? n = null, ISetupFlow? f = null)
    {
        var m = typeof(App).GetMethod("LoadSettingsAsync", BindingFlags.NonPublic | BindingFlags.Static)!;
        return await (Task<AppSettings>)m.Invoke(null, new object?[] { n, f })!;
    }

    private class RecordingFlow : ISetupFlow
    {
        public IEnvironmentService? ReceivedEnv;
        public Task<SetupData> RunAsync(string db, string cfg, IEnvironmentService? env = null)
        {
            ReceivedEnv = env;
            return Task.FromResult(new SetupData(db, cfg, new UserInfo()));
        }
    }

    [Fact]
    public async Task LoadSettingsAsync_PassesEnvironmentService_ToSetupFlow()
    {
        var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable("APPDATA", temp);
        var flow = new RecordingFlow();
        var notif = new DummyNotif();
        var env = new RecordingEnv();
        App.EnvironmentService = env;
        try
        {
            await InvokeLoadAsync(notif, flow);
            Assert.Same(env, flow.ReceivedEnv);
        }
        finally
        {
            Environment.SetEnvironmentVariable("APPDATA", null);
            App.EnvironmentService = new EnvironmentService();
        }
    }

    private class DummyNotif : INotificationService
    {
        public bool Confirm(string message) => true;
        public void ShowError(string message) { }
        public void ShowInfo(string message) { }
    }

    private class RecordingEnv : IEnvironmentService
    {
        public bool Called;
        public void Exit(int exitCode) => Called = true;
    }
}
