using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;

namespace Wrecept.Storage.Data;

public static class DataSeeder
{
    public static async Task<bool> SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);

        var hasData = await db.Products.AnyAsync(ct) || await db.Suppliers.AnyAsync(ct);
        if (hasData) return false;

        var now = DateTime.UtcNow;
        db.PaymentMethods.Add(new PaymentMethod
        {
            Id = Guid.NewGuid(),
            Name = "Készpénz",
            DueInDays = 0,
            IsArchived = false,
            CreatedAt = now,
            UpdatedAt = now
        });
        db.ProductGroups.Add(new ProductGroup { Id = Guid.NewGuid(), Name = "Általános", CreatedAt = now, UpdatedAt = now });
        db.TaxRates.Add(new TaxRate
        {
            Id = Guid.NewGuid(),
            Name = "ÁFA 27%",
            Percentage = 27m,
            EffectiveFrom = new DateTime(2020, 1, 1),
            CreatedAt = now,
            UpdatedAt = now
        });
        db.Suppliers.Add(new Supplier { Name = "Teszt Kft.", TaxId = "12345678-1-42", CreatedAt = now, UpdatedAt = now });
        db.Products.Add(new Product { Name = "Teszt termék", Net = 1000m, Gross = 1270m, CreatedAt = now, UpdatedAt = now });
        await db.SaveChangesAsync(ct);
        return true;
    }
}
