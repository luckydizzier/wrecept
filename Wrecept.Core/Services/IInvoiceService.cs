using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IInvoiceService
{
    Task AddInvoiceAsync(Invoice invoice);
    Task<IEnumerable<Invoice>> GetInvoicesAsync();
}
