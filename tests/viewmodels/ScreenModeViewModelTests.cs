using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.MAUI.Services;
using Wrecept.Core;
using Wrecept.Core.Entities;
using Wrecept.Core.Services;

namespace Wrecept.Tests.ViewModels;

public class ScreenModeViewModelTests
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
    public async Task ApplyCommand_ChangesModeAndCloses()
    {
        EnsureApp();
        var settings = new FakeSettingsService();
        var manager = new ScreenModeManager(settings);
        var vm = new ScreenModeViewModel(manager) { SelectedMode = ScreenMode.Small };
        var window = new Window();
        Application.Current.MainWindow = CreateWindow();

        await vm.ApplyCommand.ExecuteAsync(window);

        Assert.Equal(ScreenMode.Small, manager.CurrentMode);
        Assert.True(window.DialogResult);
        Assert.Equal(ScreenMode.Small, settings.Saved.ScreenMode);
    }
}
