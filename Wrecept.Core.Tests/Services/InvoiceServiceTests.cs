using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using System.Collections.Generic;
using Xunit;

namespace Wrecept.Core.Tests.Services;

public class InvoiceServiceTests
{
    private sealed class FakeInvoiceRepository : IInvoiceRepository
    {
        public Task<int> AddAsync(Invoice invoice, CancellationToken ct = default) => Task.FromResult(1);
        public Task<Invoice?> GetAsync(int id, CancellationToken ct = default) => Task.FromResult<Invoice?>(null);
        public Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default) => Task.FromResult(new List<Invoice>());
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
    public async Task CreateAsync_Allows_Negative_Quantity()
    {
        var repo = new FakeInvoiceRepository();
        var service = new InvoiceService(repo, new InvoiceCalculator());
        var invoice = new Invoice { Number = "INV1", SupplierId = 1 };
        invoice.Items.Add(new InvoiceItem { ProductId = 1, Quantity = -5, UnitPrice = 10 });

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
}
