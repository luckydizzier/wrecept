using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using Wrecept.Core;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Xunit;

namespace Wrecept.Tests;

public class ScreenModeWindowTests
{
    private class FakeSettingsService : ISettingsService
    {
        public AppSettings Saved = new();
        public Task<AppSettings> LoadAsync() => Task.FromResult(new AppSettings());
        public Task SaveAsync(AppSettings settings) { Saved = settings; return Task.CompletedTask; }
    }

    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    private static MainWindow CreateWindow() =>
        (MainWindow)FormatterServices.GetUninitializedObject(typeof(MainWindow));

    [StaFact]
    public async Task Ok_AppliesSelectedMode()
    {
        EnsureApp();
        var settings = new FakeSettingsService();
        var manager = new ScreenModeManager(settings);
        var vm = new ScreenModeViewModel(manager) { SelectedMode = ScreenMode.Small };
        var dialog = new ScreenModeWindow(vm);
        Application.Current.MainWindow = CreateWindow();

        await vm.ApplyCommand.ExecuteAsync(dialog);

        Assert.True(dialog.DialogResult);
        Assert.Equal(ScreenMode.Small, manager.CurrentMode);
        Assert.Equal(ScreenMode.Small, settings.Saved.ScreenMode);
    }

    [StaFact]
    public void Cancel_DoesNotChangeMode()
    {
        EnsureApp();
        var settings = new FakeSettingsService();
        var manager = new ScreenModeManager(settings);
        manager.CurrentMode = ScreenMode.Large;
        var vm = new ScreenModeViewModel(manager) { SelectedMode = ScreenMode.Small };
        var dialog = new ScreenModeWindow(vm);
        dialog.Close();

        Assert.Equal(ScreenMode.Large, manager.CurrentMode);
        Assert.Equal(ScreenMode.Large, settings.Saved.ScreenMode);
    }
}
