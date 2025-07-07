using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Core.Tests;

public class InvoiceServiceTests
{
    private class FakeRepo : IInvoiceRepository
    {
        public Invoice? AddedInvoice;
        public InvoiceItem? AddedItem;
        public int RemovedId;
        public Task<int> AddAsync(Invoice invoice, CancellationToken ct = default) { AddedInvoice = invoice; return Task.FromResult(1); }
        public Task<int> AddItemAsync(InvoiceItem item, CancellationToken ct = default) { AddedItem = item; return Task.FromResult(1); }
        public Task UpdateHeaderAsync(int id, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, CancellationToken ct = default) => Task.CompletedTask;
        public Task SetArchivedAsync(int id, bool isArchived, CancellationToken ct = default) => Task.CompletedTask;
        public Task<Invoice?> GetAsync(int id, CancellationToken ct = default) => Task.FromResult<Invoice?>(null);
        public Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default) => Task.FromResult(new List<Invoice>());
        public Task<LastUsageData?> GetLastUsageDataAsync(int supplierId, int productId, CancellationToken ct = default) => Task.FromResult<LastUsageData?>(null);
        public Task<Dictionary<int, LastUsageData>> GetLastUsageDataBatchAsync(int supplierId, IEnumerable<int> productIds, CancellationToken ct = default) => Task.FromResult(new Dictionary<int, LastUsageData>());
        public Task RemoveItemAsync(int id, CancellationToken ct = default) { RemovedId = id; return Task.CompletedTask; }
    }

    [Fact]
    public async Task CreateAsync_ValidInvoice_SetsDates()
    {
        var repo = new FakeRepo();
        var svc = new InvoiceService(repo, new InvoiceCalculator());
        var rate = new TaxRate { Id = Guid.NewGuid(), Percentage = 27 };
        var invoice = new Invoice
        {
            Number = "I1",
            Date = new DateOnly(2024,1,1),
            SupplierId = 1,
            PaymentMethod = new PaymentMethod { Id = Guid.NewGuid(), DueInDays = 5 },
            Items = new List<InvoiceItem>
            {
                new() { ProductId = 1, Quantity = 1, UnitPrice = 100m, TaxRate = rate }
            }
        };

        var ok = await svc.CreateAsync(invoice);

        Assert.True(ok);
        Assert.Equal(invoice, repo.AddedInvoice);
        Assert.NotEqual(DateTime.MinValue, invoice.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, invoice.Items.First().CreatedAt);
        Assert.Equal(invoice.Date.AddDays(5), invoice.DueDate);
    }

    [Fact]
    public async Task CreateAsync_Invalid_ReturnsFalse()
    {
        var repo = new FakeRepo();
        var svc = new InvoiceService(repo, new InvoiceCalculator());
        var invoice = new Invoice();

        var ok = await svc.CreateAsync(invoice);

        Assert.False(ok);
        Assert.Null(repo.AddedInvoice);
    }

    [Fact]
    public async Task AddItemAsync_SetsTimestamps()
    {
        var repo = new FakeRepo();
        var svc = new InvoiceService(repo, new InvoiceCalculator());
        var item = new InvoiceItem { InvoiceId = 1, ProductId = 2, UnitPrice = 10m, Quantity = 1, TaxRate = new TaxRate { Id = Guid.NewGuid(), Percentage = 5 }, CreatedAt = DateTime.MinValue, UpdatedAt = DateTime.MinValue };

        await svc.AddItemAsync(item);

        Assert.Equal(item, repo.AddedItem);
        Assert.NotEqual(DateTime.MinValue, repo.AddedItem!.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, repo.AddedItem!.UpdatedAt);
    }

    [Fact]
    public async Task AddItemAsync_Invalid_Throws()
    {
        var svc = new InvoiceService(new FakeRepo(), new InvoiceCalculator());
        var item = new InvoiceItem { InvoiceId = 0, ProductId = 0 };
        await Assert.ThrowsAsync<ArgumentException>(() => svc.AddItemAsync(item));
    }

    [Fact]
    public async Task RemoveItemAsync_CallsRepository()
    {
        var repo = new FakeRepo();
        var svc = new InvoiceService(repo, new InvoiceCalculator());

        await svc.RemoveItemAsync(10);

        Assert.Equal(10, repo.RemovedId);
    }
}
