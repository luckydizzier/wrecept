using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class TaxRateService : ITaxRateService
{
    private readonly ITaxRateRepository _taxRates;

    public TaxRateService(ITaxRateRepository taxRates)
    {
        _taxRates = taxRates;
    }

    public Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default)
        => _taxRates.GetAllAsync(ct);

    public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, CancellationToken ct = default)
        => _taxRates.GetActiveAsync(asOf, ct);
}
