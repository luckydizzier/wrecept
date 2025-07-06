using System.Threading;
using System.Threading.Tasks;
namespace Wrecept.Core.Services;

public class NullNumberingService : INumberingService
{
    public Task<string> GetNextInvoiceNumberAsync(CancellationToken ct = default)
        => Task.FromResult(string.Empty);
}
