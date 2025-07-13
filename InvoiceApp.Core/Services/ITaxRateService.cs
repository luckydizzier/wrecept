using InvoiceApp.Core.Models;

namespace InvoiceApp.Core.Services;

public interface ITaxRateService
{
    Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default);
    Task<List<TaxRate>> GetActiveAsync(DateTime asOf, CancellationToken ct = default);
    Task<Guid> AddAsync(TaxRate taxRate, CancellationToken ct = default);
    Task UpdateAsync(TaxRate taxRate, CancellationToken ct = default);
}
