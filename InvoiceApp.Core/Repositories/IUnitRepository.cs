using InvoiceApp.Core.Models;

namespace InvoiceApp.Core.Repositories;

public interface IUnitRepository
{
    Task<List<Unit>> GetAllAsync(CancellationToken ct = default);
    Task<List<Unit>> GetActiveAsync(CancellationToken ct = default);
    Task<Guid> AddAsync(Unit unit, CancellationToken ct = default);
    Task UpdateAsync(Unit unit, CancellationToken ct = default);
}
