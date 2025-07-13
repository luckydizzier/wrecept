using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Data.Services;
using Xunit;

namespace InvoiceApp.Core.Tests.Services;

public class NumberingServiceTests
{
    private sealed class FakeInvoiceRepository : IInvoiceRepository
    {
        public string? Latest;
        public int Supplier;
        public Task<int> AddAsync(InvoiceApp.Core.Models.Invoice invoice, CancellationToken ct = default) => Task.FromResult(0);
        public Task<int> AddItemAsync(InvoiceApp.Core.Models.InvoiceItem item, CancellationToken ct = default) => Task.FromResult(0);
        public Task RemoveItemAsync(int id, CancellationToken ct = default) => Task.CompletedTask;
        public Task UpdateHeaderAsync(int id, string number, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, CancellationToken ct = default) => Task.CompletedTask;
        public Task SetArchivedAsync(int id, bool isArchived, CancellationToken ct = default) => Task.CompletedTask;
        public Task<InvoiceApp.Core.Models.Invoice?> GetAsync(int id, CancellationToken ct = default) => Task.FromResult<InvoiceApp.Core.Models.Invoice?>(null);
        public Task<List<InvoiceApp.Core.Models.Invoice>> GetRecentAsync(int count, CancellationToken ct = default) => Task.FromResult(new List<InvoiceApp.Core.Models.Invoice>());
        public Task<LastUsageData?> GetLastUsageDataAsync(int supplierId, int productId, CancellationToken ct = default) => Task.FromResult<LastUsageData?>(null);
        public Task<Dictionary<int, LastUsageData>> GetLastUsageDataBatchAsync(int supplierId, IEnumerable<int> productIds, CancellationToken ct = default) => Task.FromResult(new Dictionary<int, LastUsageData>());
        public Task<string?> GetLatestInvoiceNumberBySupplierAsync(int supplierId, CancellationToken ct = default)
        {
            Supplier = supplierId;
            return Task.FromResult(Latest);
        }
    }

    [Fact]
    public async Task GeneratesSequentialNumber_FromLatest()
    {
        var repo = new FakeInvoiceRepository { Latest = "INV5" };
        var svc = new NumberingService(repo);

        var next = await svc.GetNextInvoiceNumberAsync(1);

        Assert.Equal("INV6", next);
        Assert.Equal(1, repo.Supplier);
    }

    [Fact]
    public async Task ReturnsInv1_WhenNoPrevious()
    {
        var repo = new FakeInvoiceRepository { Latest = null };
        var svc = new NumberingService(repo);

        var next = await svc.GetNextInvoiceNumberAsync(2);

        Assert.Equal("INV1", next);
    }

    [Fact]
    public async Task KeepsPrefixAndSuffix()
    {
        var repo = new FakeInvoiceRepository { Latest = "ABC-001/2024" };
        var svc = new NumberingService(repo);

        var next = await svc.GetNextInvoiceNumberAsync(3);

        Assert.Equal("ABC-002/2024", next);
    }
}
