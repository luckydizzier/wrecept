using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.UI.Services;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Tests;

public class InvoiceEditorViewModelTests
{
    [SkippableFact]
    [Trait("Category", "UI")]
    public void DeleteItem_UsesMessageServiceConfirm()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "UI tests require Windows");

        var invoiceService = new TestInvoiceService();
        var suggestionService = new TestSuggestionIndexService();
        var messageService = new TestMessageService();
        var vm = new InvoiceEditorViewModel(invoiceService, suggestionService, messageService);

        var item = new InvoiceItem();
        vm.Items.Add(item);
        vm.SelectedItem = item;

        vm.DeleteItemCommand.Execute(null);

        Assert.True(messageService.ConfirmCalled);
        Assert.Empty(vm.Items);
    }

    private class TestInvoiceService : IInvoiceService
    {
        public Task AddInvoiceAsync(Invoice invoice) => Task.CompletedTask;
        public Task<IEnumerable<Invoice>> GetInvoicesAsync() => Task.FromResult<IEnumerable<Invoice>>(Array.Empty<Invoice>());
    }

    private class TestSuggestionIndexService : ISuggestionIndexService
    {
        public Task AddHistoryEntryAsync(string term) => Task.CompletedTask;
        public Task<IReadOnlyList<string>> GetPredictionsAsync(string prefix) => Task.FromResult<IReadOnlyList<string>>(Array.Empty<string>());
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
