using System.Threading;
using System.Threading.Tasks;

namespace InvoiceApp.Core.Services;

public interface INumberingService
{
    Task<string> GetNextInvoiceNumberAsync(int supplierId, CancellationToken ct = default);
}
