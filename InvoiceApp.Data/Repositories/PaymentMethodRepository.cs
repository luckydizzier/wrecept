using Microsoft.EntityFrameworkCore;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Data.Data;
using System.Text.Json;

namespace InvoiceApp.Data.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly AppDbContext _db;

    public PaymentMethodRepository(AppDbContext db)
    {
        _db = db;
    }

    private void Log(string id, string op, object data)
    {
        _db.ChangeLogs.Add(new ChangeLog
        {
            Entity = nameof(PaymentMethod),
            EntityId = id,
            Operation = op,
            Data = JsonSerializer.Serialize(data),
            CreatedAt = DateTime.UtcNow
        });
    }

    public Task<List<PaymentMethod>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<PaymentMethod>().AsNoTracking().ToListAsync(ct);

    public Task<List<PaymentMethod>> GetActiveAsync(CancellationToken ct = default)
        => _db.Set<PaymentMethod>().AsNoTracking().Where(m => !m.IsArchived).ToListAsync(ct);

    public async Task<Guid> AddAsync(PaymentMethod method, CancellationToken ct = default)
    {
        _db.Add(method);
        await _db.SaveChangesAsync(ct);
        Log(method.Id.ToString(), "Insert", method);
        await _db.SaveChangesAsync(ct);
        return method.Id;
    }

    public async Task UpdateAsync(PaymentMethod method, CancellationToken ct = default)
    {
        _db.Update(method);
        await _db.SaveChangesAsync(ct);
        Log(method.Id.ToString(), "Update", method);
        await _db.SaveChangesAsync(ct);
    }
}
