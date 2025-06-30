using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Storage.Data;

namespace Wrecept.Storage.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly AppDbContext _db;

    public PaymentMethodRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<PaymentMethod>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<PaymentMethod>().ToListAsync(ct);

    public Task<List<PaymentMethod>> GetActiveAsync(CancellationToken ct = default)
        => _db.Set<PaymentMethod>().Where(m => !m.IsArchived).ToListAsync(ct);

    public async Task<Guid> AddAsync(PaymentMethod method, CancellationToken ct = default)
    {
        _db.Add(method);
        await _db.SaveChangesAsync(ct);
        return method.Id;
    }

    public async Task UpdateAsync(PaymentMethod method, CancellationToken ct = default)
    {
        _db.Update(method);
        await _db.SaveChangesAsync(ct);
    }
}
