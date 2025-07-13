using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Tests.ViewModels;

public class InvoiceLookupViewModelTests
{
    private class FakeInvoiceService : IInvoiceService
    {
        public List<Invoice> Invoices { get; } = new();
        public Task<List<Invoice>> GetRecentAsync(int count, System.Threading.CancellationToken ct = default)
            => Task.FromResult(Invoices);

        public Task<bool> CreateAsync(Invoice invoice, System.Threading.CancellationToken ct = default)
            => throw new NotImplementedException();
        public Task<int> CreateHeaderAsync(Invoice invoice, System.Threading.CancellationToken ct = default)
            => throw new NotImplementedException();
        public Task<int> AddItemAsync(InvoiceItem item, System.Threading.CancellationToken ct = default)
            => throw new NotImplementedException();
        public Task UpdateInvoiceHeaderAsync(int id, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, System.Threading.CancellationToken ct = default)
            => throw new NotImplementedException();
        public Task ArchiveAsync(int id, System.Threading.CancellationToken ct = default)
            => throw new NotImplementedException();
        public Task<Invoice?> GetAsync(int id, System.Threading.CancellationToken ct = default)
            => throw new NotImplementedException();
        public Task<LastUsageData?> GetLastUsageDataAsync(int supplierId, int productId, System.Threading.CancellationToken ct = default)
            => throw new NotImplementedException();
        public InvoiceCalculationResult RecalculateTotals(Invoice invoice)
            => throw new NotImplementedException();
    }

    private class FakeNumberingService : INumberingService
    {
        private int _counter;
        public Task<string> GetNextInvoiceNumberAsync(int supplierId, System.Threading.CancellationToken ct = default)
        {
            _counter++;
            return Task.FromResult($"INV{_counter}");
        }
    }

    [Fact]
    public async Task LoadAsync_SelectsFirstInvoice()
    {
        var service = new FakeInvoiceService();
        service.Invoices.Add(new Invoice { Id = 1, Number = "A1", Date = DateOnly.FromDateTime(DateTime.Today) });
        service.Invoices.Add(new Invoice { Id = 2, Number = "A2", Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)) });

        var vm = new InvoiceLookupViewModel(service, new FakeNumberingService());
        await vm.LoadAsync();

        Assert.Equal(1, vm.SelectedInvoice?.Id);
    }
}
