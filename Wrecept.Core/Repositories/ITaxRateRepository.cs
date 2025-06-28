using Wrecept.Core.Entities;

namespace Wrecept.Core.Repositories;

public interface ITaxRateRepository
{
    Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default);
}
