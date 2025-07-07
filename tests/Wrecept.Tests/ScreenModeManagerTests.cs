using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using Wrecept.Core;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;
using Wrecept.Wpf;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class ScreenModeManagerTests
{
    private class FakeSettingsService : ISettingsService
    {
        public AppSettings Saved = new();
        public AppSettings LoadValue = new();
        public Task<AppSettings> LoadAsync() => Task.FromResult(LoadValue);
        public Task SaveAsync(AppSettings settings)
        {
            Saved = settings;
            return Task.CompletedTask;
        }
    }

    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    private static MainWindow CreateWindow() =>
        (MainWindow)FormatterServices.GetUninitializedObject(typeof(MainWindow));

    [StaFact]
    public async Task ApplySavedAsync_LoadsAndApplies()
    {
        EnsureApp();
        var settings = new FakeSettingsService { LoadValue = new AppSettings { ScreenMode = ScreenMode.Small } };
        var manager = new ScreenModeManager(settings);
        var window = CreateWindow();

        await manager.ApplySavedAsync(window);

        Assert.Equal(ScreenMode.Small, manager.CurrentMode);
        Assert.Equal(800d, window.Width);
        Assert.Equal(600d, window.Height);
    }

    [StaFact]
    public async Task ChangeModeAsync_ResizesAndSaves()
    {
        EnsureApp();
        var settings = new FakeSettingsService();
        var manager = new ScreenModeManager(settings);
        var window = CreateWindow();

        await manager.ChangeModeAsync(window, ScreenMode.ExtraLarge);

        Assert.Equal(ScreenMode.ExtraLarge, manager.CurrentMode);
        Assert.Equal(1920d, window.Width);
        Assert.Equal(1080d, window.Height);
        Assert.Equal(ScreenMode.ExtraLarge, settings.Saved.ScreenMode);
    }

    [StaFact]
    public async Task ChangeModeAsync_SameMode_NoOp()
    {
        EnsureApp();
        var settings = new FakeSettingsService { LoadValue = new AppSettings { ScreenMode = ScreenMode.Small } };
        var manager = new ScreenModeManager(settings);
        var window = CreateWindow();

        await manager.ApplySavedAsync(window);
        await manager.ChangeModeAsync(window, ScreenMode.Small);

        Assert.Equal(ScreenMode.Small, manager.CurrentMode);
        Assert.Equal(ScreenMode.Large, settings.Saved.ScreenMode);
        Assert.Equal(800d, window.Width);
        Assert.Equal(600d, window.Height);
    }
}
