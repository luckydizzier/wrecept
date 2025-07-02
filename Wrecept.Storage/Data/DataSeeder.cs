using Bogus;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Core.Utilities;

namespace Wrecept.Storage.Data;

public static class DataSeeder
{
    public static async Task<bool> IsDatabaseEmptyAsync(string dbPath, ILogService logService, CancellationToken ct = default)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;
        await using var db = new AppDbContext(opts);
        await DbInitializer.EnsureCreatedAndMigratedAsync(db, logService, ct);
        try
        {
            var hasInvoice = await db.Invoices.AnyAsync(ct);
            var hasProduct = await db.Products.AnyAsync(ct);
            var hasSupplier = await db.Suppliers.AnyAsync(ct);
            return !(hasInvoice || hasProduct || hasSupplier);
        }
        catch (SqliteException ex)
        {
            await logService.LogError("Empty check failed", ex);
            return true;
        }
    }

    public static async Task<SeedStatus> SeedSampleDataAsync(
        string dbPath,
        ILogService logService,
        IProgress<ProgressReport>? progress = null,
        CancellationToken ct = default)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;
        await using var db = new AppDbContext(opts);
        await DbInitializer.EnsureCreatedAndMigratedAsync(db, logService, ct);

        var faker = new Faker("en_GB");
        var now = DateTime.UtcNow;

        progress?.Report(new ProgressReport { GlobalPercent = 10, Message = "Alap adatok..." });

        var payments = new[]
        {
            new PaymentMethod { Id = Guid.NewGuid(), Name = "Készpénz", DueInDays = 0, CreatedAt = now, UpdatedAt = now },
            new PaymentMethod { Id = Guid.NewGuid(), Name = "Átutalás", DueInDays = 8, CreatedAt = now, UpdatedAt = now }
        };
        var units = new[]
        {
            new Unit { Id = Guid.NewGuid(), Code = "DB", Name = "db", CreatedAt = now, UpdatedAt = now },
            new Unit { Id = Guid.NewGuid(), Code = "KG", Name = "kg", CreatedAt = now, UpdatedAt = now }
        };
        var group = new ProductGroup { Id = Guid.NewGuid(), Name = "Általános", CreatedAt = now, UpdatedAt = now };
        var taxes = new[]
        {
            new TaxRate { Id = Guid.NewGuid(), Code = "A27", Name = "ÁFA 27%", Percentage = 27m, EffectiveFrom = new DateTime(2020,1,1), CreatedAt = now, UpdatedAt = now },
            new TaxRate { Id = Guid.NewGuid(), Code = "A5", Name = "ÁFA 5%", Percentage = 5m, EffectiveFrom = new DateTime(2020,1,1), CreatedAt = now, UpdatedAt = now }
        };
        db.PaymentMethods.AddRange(payments);
        db.Units.AddRange(units);
        db.ProductGroups.Add(group);
        db.TaxRates.AddRange(taxes);
        await db.SaveChangesAsync(ct);

        progress?.Report(new ProgressReport { GlobalPercent = 30, Message = "Szállítók..." });

        var supplierFaker = new Faker<Supplier>("en_GB")
            .RuleFor(s => s.Name, f => f.Company.CompanyName())
            .RuleFor(s => s.TaxId, f => f.Random.Replace("########-#-##"))
            .RuleFor(s => s.CreatedAt, _ => now)
            .RuleFor(s => s.UpdatedAt, _ => now);
        var suppliers = supplierFaker.Generate(20);
        db.Suppliers.AddRange(suppliers);
        await db.SaveChangesAsync(ct);

        progress?.Report(new ProgressReport { GlobalPercent = 50, Message = "Termékek..." });

        var productFaker = new Faker<Product>("en_GB")
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Net, f => Math.Round(f.Random.Decimal(100, 10000), 2))
            .RuleFor(p => p.TaxRateId, f => f.PickRandom(taxes).Id)
            .RuleFor(p => p.UnitId, f => f.PickRandom(units).Id)
            .RuleFor(p => p.ProductGroupId, _ => group.Id)
            .RuleFor(p => p.CreatedAt, _ => now)
            .RuleFor(p => p.UpdatedAt, _ => now)
            .FinishWith((f, p) =>
            {
                var tax = taxes.First(t => t.Id == p.TaxRateId).Percentage;
                p.Gross = Math.Round(p.Net * (1 + tax / 100m), 2);
            });
        var products = productFaker.Generate(500);
        db.Products.AddRange(products);
        await db.SaveChangesAsync(ct);

        progress?.Report(new ProgressReport { GlobalPercent = 70, Message = "Számlák..." });

        var invoiceFaker = new Faker<Invoice>("en_GB")
            .RuleFor(i => i.Number, f => f.Random.Replace("INV-#####"))
            .RuleFor(i => i.Date, f => DateOnly.FromDateTime(f.Date.Recent(365)))
            .RuleFor(i => i.SupplierId, f => f.PickRandom(suppliers).Id)
            .RuleFor(i => i.PaymentMethodId, f => f.PickRandom(payments).Id)
            .RuleFor(i => i.IsGross, _ => false)
            .RuleFor(i => i.CreatedAt, _ => now)
            .RuleFor(i => i.UpdatedAt, _ => now);
        var invoices = invoiceFaker.Generate(100);
        db.Invoices.AddRange(invoices);
        await db.SaveChangesAsync(ct);

        progress?.Report(new ProgressReport { GlobalPercent = 90, Message = "Tételek..." });

        foreach (var invoice in invoices)
        {
            var itemFaker = new Faker<InvoiceItem>("en_GB")
                .RuleFor(it => it.InvoiceId, _ => invoice.Id)
                .RuleFor(it => it.ProductId, f => f.PickRandom(products).Id)
                .RuleFor(it => it.Description, (f, it) => products.First(p => p.Id == it.ProductId).Name)
                .RuleFor(it => it.TaxRateId, (f, it) => products.First(p => p.Id == it.ProductId).TaxRateId)
                .RuleFor(it => it.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(it => it.UnitPrice, (f, it) => products.First(p => p.Id == it.ProductId).Net)
                .RuleFor(it => it.CreatedAt, _ => now)
                .RuleFor(it => it.UpdatedAt, _ => now);
            var count = faker.Random.Int(5, 60);
            db.InvoiceItems.AddRange(itemFaker.Generate(count));
        }
        await db.SaveChangesAsync(ct);

        progress?.Report(new ProgressReport { GlobalPercent = 100, SubtaskPercent = 100, Message = "Kész." });
        return SeedStatus.Seeded;
    }
}
