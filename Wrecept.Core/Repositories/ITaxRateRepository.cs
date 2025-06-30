using Wrecept.Core.Models;

namespace Wrecept.Core.Repositories;

public interface ITaxRateRepository
{
    Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default);
    Task<List<TaxRate>> GetActiveAsync(DateTime asOf, CancellationToken ct = default);
}
