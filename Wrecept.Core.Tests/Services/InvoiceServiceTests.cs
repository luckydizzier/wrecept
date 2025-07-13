using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Core.Services;
using System.Collections.Generic;
using Xunit;

namespace InvoiceApp.Core.Tests.Services;

public class InvoiceServiceTests
{
    private sealed class FakeInvoiceRepository : IInvoiceRepository
    {
        public Task<int> AddAsync(Invoice invoice, CancellationToken ct = default) => Task.FromResult(1);
        public Task<int> AddItemAsync(InvoiceItem item, CancellationToken ct = default) => Task.FromResult(1);
        public Task UpdateHeaderAsync(int id, string number, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, CancellationToken ct = default)
        {
            UpdatedId = id;
            Number = number;
            HeaderDate = date;
            HeaderDueDate = dueDate;
            Supplier = supplierId;
            Payment = paymentMethodId;
            Gross = isGross;
            return Task.CompletedTask;
        }
        public Task SetArchivedAsync(int id, bool isArchived, CancellationToken ct = default)
        {
            Archived = isArchived;
            return Task.CompletedTask;
        }
        public Task<Invoice?> GetAsync(int id, CancellationToken ct = default) => Task.FromResult<Invoice?>(null);
        public Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default) => Task.FromResult(new List<Invoice>());

        public Task<LastUsageData?> GetLastUsageDataAsync(int supplierId, int productId, CancellationToken ct = default)
            => Task.FromResult<LastUsageData?>(null);

        public Task<Dictionary<int, LastUsageData>> GetLastUsageDataBatchAsync(int supplierId, IEnumerable<int> productIds, CancellationToken ct = default)
            => Task.FromResult(new Dictionary<int, LastUsageData>());

        public int UpdatedId;
        public bool Archived;
        public string? Number;
        public DateOnly HeaderDate;
        public DateOnly HeaderDueDate;
        public int Supplier;
        public Guid Payment;
        public bool Gross;
    }

    [Fact]
    public async Task CreateAsync_ReturnsFalse_WhenItemQuantityIsZero()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());
        var invoice = new Invoice { Number = "INV1", SupplierId = 1 };
        invoice.Items.Add(new InvoiceItem { ProductId = 1, Quantity = 0, UnitPrice = 10 });

        var result = await service.CreateAsync(invoice);

        Assert.False(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsFalse_WhenNumberEmpty()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());
        var invoice = new Invoice { Number = "", SupplierId = 1 };
        invoice.Items.Add(new InvoiceItem { ProductId = 1, Quantity = 1, UnitPrice = 10, TaxRate = new TaxRate { Id = Guid.NewGuid(), Percentage = 27 } });

        var result = await service.CreateAsync(invoice);

        Assert.False(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsFalse_WhenItemMissingProductOrTax()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());
        var invoice = new Invoice { Number = "INV1", SupplierId = 1 };
        invoice.Items.Add(new InvoiceItem { Quantity = 1, UnitPrice = 10 });

        var result = await service.CreateAsync(invoice);

        Assert.False(result);
    }

    [Fact]
    public async Task AddItemAsync_Throws_OnInvalidIds()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());
        var item = new InvoiceItem { InvoiceId = 0, ProductId = 0, Quantity = 1, UnitPrice = 10 };

        await Assert.ThrowsAsync<ArgumentException>(() => service.AddItemAsync(item));
    }

    [Fact]
    public async Task CreateAsync_Allows_Negative_Quantity()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());
        var invoice = new Invoice { Number = "INV1", SupplierId = 1 };
        invoice.Items.Add(new InvoiceItem
        {
            ProductId = 1,
            Quantity = -5,
            UnitPrice = 10,
            TaxRate = new TaxRate { Id = Guid.NewGuid(), Percentage = 27 }
        });

        var result = await service.CreateAsync(invoice);

        Assert.True(result);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsList()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());

        var list = await service.GetRecentAsync(5);

        Assert.NotNull(list);
    }

    [Fact]
    public async Task UpdateInvoiceHeaderAsync_Throws_WhenMissingFields()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateInvoiceHeaderAsync(0, DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today), 1, Guid.NewGuid(), true));
    }

    [Fact]
    public async Task UpdateInvoiceHeaderAsync_UpdatesRepository()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());

        var date = new DateOnly(2024, 2, 2);
        var due = new DateOnly(2024, 2, 12);
        var pm = Guid.NewGuid();

        await service.UpdateInvoiceHeaderAsync(7, "INV3", date, due, 4, pm, false);

        Assert.Equal(7, repo.UpdatedId);
        Assert.Equal("INV3", repo.Number);
        Assert.Equal(date, repo.HeaderDate);
        Assert.Equal(due, repo.HeaderDueDate);
        Assert.Equal(4, repo.Supplier);
        Assert.Equal(pm, repo.Payment);
        Assert.False(repo.Gross);
    }

    [Fact]
    public async Task ArchiveAsync_SetsFlag()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());

        await service.ArchiveAsync(5);

        Assert.True(repo.Archived);
    }
}
