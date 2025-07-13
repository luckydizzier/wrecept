using InvoiceApp.Core.Models;

namespace InvoiceApp.Core.Repositories;

public interface ITaxRateRepository
{
    Task<List<TaxRate>> GetAllAsync(CancellationToken ct = default);
    Task<List<TaxRate>> GetActiveAsync(DateTime asOf, CancellationToken ct = default);
    Task<Guid> AddAsync(TaxRate taxRate, CancellationToken ct = default);
    Task UpdateAsync(TaxRate taxRate, CancellationToken ct = default);
}
