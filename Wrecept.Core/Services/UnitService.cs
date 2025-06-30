using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Services;

public class UnitService : IUnitService
{
    private readonly IUnitRepository _units;

    public UnitService(IUnitRepository units)
    {
        _units = units;
    }

    public Task<List<Unit>> GetAllAsync(CancellationToken ct = default)
        => _units.GetAllAsync(ct);

    public Task<List<Unit>> GetActiveAsync(CancellationToken ct = default)
        => _units.GetActiveAsync(ct);

    public async Task<Guid> AddAsync(Unit unit, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(unit);
        if (string.IsNullOrWhiteSpace(unit.Name))
            throw new ArgumentException("Name required", nameof(unit));

        unit.CreatedAt = DateTime.UtcNow;
        unit.UpdatedAt = DateTime.UtcNow;
        return await _units.AddAsync(unit, ct);
    }

    public async Task UpdateAsync(Unit unit, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(unit);
        if (unit.Id == Guid.Empty)
            throw new ArgumentException("Invalid Id", nameof(unit));
        if (string.IsNullOrWhiteSpace(unit.Name))
            throw new ArgumentException("Name required", nameof(unit));

        unit.UpdatedAt = DateTime.UtcNow;
        await _units.UpdateAsync(unit, ct);
    }
}
