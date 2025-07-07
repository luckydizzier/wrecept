using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Wpf.Services;

namespace Wrecept.Tests.ViewModels;

public class InvoiceCreatePromptViewModelTests
{
    private class FakeInvoiceService : IInvoiceService
    {
        public readonly List<Invoice> Created = new();
        public Task<bool> CreateAsync(Invoice invoice, System.Threading.CancellationToken ct = default)
        {
            Created.Add(invoice);
            return Task.FromResult(true);
        }
        public Task<int> CreateHeaderAsync(Invoice invoice, System.Threading.CancellationToken ct = default)
        {
            Created.Add(invoice);
            return Task.FromResult(1);
        }
        public Task<int> AddItemAsync(InvoiceItem item, System.Threading.CancellationToken ct = default) => Task.FromResult(1);
        public Task UpdateInvoiceHeaderAsync(int id, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
        public Task ArchiveAsync(int id, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
        public Task<Invoice?> GetAsync(int id, System.Threading.CancellationToken ct = default) => Task.FromResult<Invoice?>(null);
        public Task<List<Invoice>> GetRecentAsync(int count, System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Invoice>());
        public InvoiceCalculationResult RecalculateTotals(Invoice invoice) => new();
    }

    [Fact]
    public async Task ConfirmAsync_CreatesEntryAndCloses()
    {
        var service = new FakeInvoiceService();
        var lookup = new InvoiceLookupViewModel(service, new FakeNumberingService(), new AppStateService(Path.GetTempFileName()));
        var prompt = new InvoiceCreatePromptViewModel(lookup, "A1");
        lookup.InlinePrompt = prompt;

        await prompt.ConfirmAsyncCommand.ExecuteAsync(null);

        Assert.Null(lookup.InlinePrompt);
        Assert.Single(lookup.Invoices);
    }

    [Fact]
    public void Cancel_ClosesPrompt()
    {
        var service = new FakeInvoiceService();
        var lookup = new InvoiceLookupViewModel(service, new FakeNumberingService(), new AppStateService(Path.GetTempFileName()));
        var prompt = new InvoiceCreatePromptViewModel(lookup, "A1");
        lookup.InlinePrompt = prompt;

        prompt.CancelCommand.Execute(null);

        Assert.Null(lookup.InlinePrompt);
    }
}
