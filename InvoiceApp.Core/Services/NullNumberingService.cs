using System.Threading;
using System.Threading.Tasks;
namespace InvoiceApp.Core.Services;

public class NullNumberingService : INumberingService
{
    public Task<string> GetNextInvoiceNumberAsync(int supplierId, CancellationToken ct = default)
        => Task.FromResult(string.Empty);
}
