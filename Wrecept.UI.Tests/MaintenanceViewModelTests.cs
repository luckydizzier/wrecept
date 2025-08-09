using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Wrecept.Core.Services;
using Wrecept.UI.Services;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Tests;

public class MaintenanceViewModelTests
{
    [SkippableFact]
    [Trait("Category", "UI")]
    public async Task ExportAsync_UsesMessageService()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "UI tests require Windows");

        var exportService = new TestExportService();
        var messageService = new TestMessageService();
        var vm = new MaintenanceViewModel(exportService, messageService);

        vm.ExportCommand.Execute(null);
        await exportService.Completed.Task;

        Assert.True(messageService.ConfirmCalled);
        Assert.True(messageService.ShowCalled);
    }

    private class TestExportService : IExportService
    {
        public TaskCompletionSource Completed { get; } = new();

        public Task ExportAsync(string path)
        {
            Completed.SetResult();
            return Task.CompletedTask;
        }

        public Task ImportAsync(string path) => Task.CompletedTask;
    }

    private class TestMessageService : IMessageService
    {
        public bool ConfirmCalled { get; private set; }
        public bool ShowCalled { get; private set; }

        public void Show(string message, string caption = "Information") => ShowCalled = true;

        public bool Confirm(string message, string caption = "Confirm")
        {
            ConfirmCalled = true;
            return true;
        }
    }
}
