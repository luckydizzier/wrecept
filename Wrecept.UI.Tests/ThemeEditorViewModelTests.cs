using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.UI.Services;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Tests;

public class ThemeEditorViewModelTests
{
    [SkippableFact]
    [Trait("Category", "UI")]
    public async Task InitializeAsync_LoadsTheme()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "UI tests require Windows");

        var settingsService = new TestSettingsService();
        var messageService = new TestMessageService();
        var vm = new ThemeEditorViewModel(settingsService, messageService);

        await vm.InitializeAsync();

        Assert.Equal("Dark", vm.SelectedTheme);
    }

    private class TestSettingsService : ISettingsService
    {
        public event EventHandler<ApplicationSettings>? SettingsChanged;
        public Task<ApplicationSettings> LoadAsync() => Task.FromResult(new ApplicationSettings { Theme = "Dark" });
        public Task SaveAsync(ApplicationSettings settings) => Task.CompletedTask;
        public Task UpdateThemeAsync(string theme) => Task.CompletedTask;
        public Task UpdateLanguageAsync(string language) => Task.CompletedTask;
    }

    private class TestMessageService : IMessageService
    {
        public void Show(string message, string caption = "Information") { }
        public bool Confirm(string message, string caption = "Confirm") => true;
    }
}
