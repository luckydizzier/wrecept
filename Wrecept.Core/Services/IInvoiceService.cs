using System.Collections.Generic;
using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IInvoiceService
{
    Task<bool> CreateAsync(Invoice invoice, CancellationToken ct = default);
    Task<int> CreateHeaderAsync(Invoice invoice, CancellationToken ct = default);
    Task<int> AddItemAsync(InvoiceItem item, CancellationToken ct = default);
    Task RemoveItemAsync(int id, CancellationToken ct = default);
    Task UpdateInvoiceHeaderAsync(int id, string number, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, CancellationToken ct = default);
    Task ArchiveAsync(int id, CancellationToken ct = default);
    Task<Invoice?> GetAsync(int id, CancellationToken ct = default);
    Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default);

    Task<LastUsageData?> GetLastUsageDataAsync(int supplierId, int productId, CancellationToken ct = default);

    Task<Dictionary<int, LastUsageData>> GetLastUsageDataBatchAsync(int supplierId, IEnumerable<int> productIds, CancellationToken ct = default);

    InvoiceCalculationResult RecalculateTotals(Invoice invoice);
}
