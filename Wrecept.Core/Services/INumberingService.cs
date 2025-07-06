using System.Threading;
using System.Threading.Tasks;

namespace Wrecept.Core.Services;

public interface INumberingService
{
    Task<string> GetNextInvoiceNumberAsync(CancellationToken ct = default);
}
