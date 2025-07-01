using Wrecept.Core.Models;

namespace Wrecept.Core.Repositories;

public interface IInvoiceRepository
{
    Task<int> AddAsync(Invoice invoice, CancellationToken ct = default);
    Task<int> AddItemAsync(InvoiceItem item, CancellationToken ct = default);
    Task<Invoice?> GetAsync(int id, CancellationToken ct = default);
    Task<List<Invoice>> GetRecentAsync(int count, CancellationToken ct = default);
}
