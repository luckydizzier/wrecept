using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IInvoiceService
{
    Task<bool> CreateAsync(Invoice invoice, CancellationToken ct = default);
    Task<int> CreateHeaderAsync(Invoice invoice, CancellationToken ct = default);
    Task<int> AddItemAsync(InvoiceItem item, CancellationToken ct = default);
    Task UpdateInvoiceHeaderAsync(int id, DateOnly date, int supplierId, Guid paymentMethodId, bool isGross, CancellationToken ct = default);
    Task ArchiveAsync(int id, CancellationToken ct = default);
    Task<Invoice?> GetAsync(int id, CancellationToken ct = default);
    Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default);
}
