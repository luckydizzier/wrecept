using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;
using Wrecept.Wpf;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.Views;
using Xunit;

namespace Wrecept.Tests;

public class MainWindowTests
{
    private class FakeSettingsService : ISettingsService
    {
        public bool LoadCalled;
        public Task<AppSettings> LoadAsync() { LoadCalled = true; return Task.FromResult(new AppSettings()); }
        public Task SaveAsync(AppSettings settings) => Task.CompletedTask;
    }

    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    private static StageView CreateStageView() =>
        (StageView)FormatterServices.GetUninitializedObject(typeof(StageView));

    [StaFact]
    public void Loaded_InvokesScreenModeManager()
    {
        EnsureApp();
        var settings = new FakeSettingsService();
        var manager = new ScreenModeManager(settings);
        var window = new MainWindow(CreateStageView(), manager);
        window.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        window.Dispatcher.Invoke(() => { }, DispatcherPriority.ContextIdle);
        Assert.True(settings.LoadCalled);
    }
}
