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

    public async Task<Guid> AddAsync(TaxRate taxRate, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(taxRate);
        if (string.IsNullOrWhiteSpace(taxRate.Name))
            throw new ArgumentException("Name required", nameof(taxRate));

        taxRate.CreatedAt = DateTime.UtcNow;
        taxRate.UpdatedAt = DateTime.UtcNow;
        return await _taxRates.AddAsync(taxRate, ct);
    }

    public async Task UpdateAsync(TaxRate taxRate, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(taxRate);
        if (taxRate.Id == Guid.Empty)
            throw new ArgumentException("Invalid Id", nameof(taxRate));
        if (string.IsNullOrWhiteSpace(taxRate.Name))
            throw new ArgumentException("Name required", nameof(taxRate));

        taxRate.UpdatedAt = DateTime.UtcNow;
        await _taxRates.UpdateAsync(taxRate, ct);
    }
}
