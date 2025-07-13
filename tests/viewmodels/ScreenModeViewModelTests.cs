using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.MAUI.Services;
using InvoiceApp.Core;
using InvoiceApp.Core.Entities;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Tests.ViewModels;

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
            _ = new Application();
    }

    [StaFact]
    public async Task ApplyCommand_ChangesModeAndCloses()
    {
        EnsureApp();
        var settings = new FakeSettingsService();
        var manager = new ScreenModeManager(settings);
        var vm = new ScreenModeViewModel(manager) { SelectedMode = ScreenMode.Small };
        var window = new Window();

        await vm.ApplyCommand.ExecuteAsync(window);

        Assert.Equal(ScreenMode.Small, manager.CurrentMode);
        Assert.Equal(ScreenMode.Small, settings.Saved.ScreenMode);
    }
}
