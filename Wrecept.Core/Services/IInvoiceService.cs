using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IInvoiceService
{
    Task<bool> CreateAsync(Invoice invoice, CancellationToken ct = default);
    Task<Invoice?> GetAsync(int id, CancellationToken ct = default);
}
