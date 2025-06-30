using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public interface IUnitService
{
    Task<List<Unit>> GetAllAsync(CancellationToken ct = default);
    Task<List<Unit>> GetActiveAsync(CancellationToken ct = default);
    Task<Guid> AddAsync(Unit unit, CancellationToken ct = default);
    Task UpdateAsync(Unit unit, CancellationToken ct = default);
}
