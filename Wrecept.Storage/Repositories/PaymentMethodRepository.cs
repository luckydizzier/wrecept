using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Entities;
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
}
