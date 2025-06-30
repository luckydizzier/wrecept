using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface ITaxRateService
{
    Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default);
    Task<TaxRate?> GetActiveAsync(DateTime asOf, CancellationToken ct = default);
}
