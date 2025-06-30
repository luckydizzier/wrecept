using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Models;
using System.IO;
using Wrecept.Core.Services;
using Wrecept.Core.Utilities;

namespace Wrecept.Storage.Data;

public static class DataSeeder
{
    private const string SampleSupplier = "Teszt Kft.";
    private const string SampleSupplier2 = "Minta Bt.";
    private const string SampleProduct = "Teszt termék";
    private const string SampleProduct2 = "Második termék";
    private const string SampleGroup = "Általános";
    private const string SampleTax = "ÁFA 27%";
    private const string SampleTax2 = "ÁFA 5%";
    private const string SamplePayment = "Készpénz";
    private const string SamplePayment2 = "Átutalás";
    private const string SampleUnit = "db";
    private const string SampleUnit2 = "kg";

    public static async Task<SeedStatus> SeedAsync(
        string dbPath,
        ILogService logService,
        IProgress<ProgressReport>? progress = null,
        CancellationToken ct = default)
    {
        _ = File.Exists(dbPath);

        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        await using var ctx = new AppDbContext(opts);
        progress?.Report(new ProgressReport { GlobalPercent = 20, Message = "Migráció ellenőrzése..." });
        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, logService, ct);
        progress?.Report(new ProgressReport { GlobalPercent = 40, Message = "Adatok ellenőrzése..." });

        bool hasData;
        try
        {
            hasData = await ctx.Products.AnyAsync(ct) || await ctx.Suppliers.AnyAsync(ct);
        }
        catch (Exception)
        {
            await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, logService, ct);
            try
            {
                await using var retryCtx = new AppDbContext(opts);
                hasData = await retryCtx.Products.AnyAsync(ct) || await retryCtx.Suppliers.AnyAsync(ct);
            }
            catch (SqliteException ex)
            {
                await logService.LogError("Startup data check failed", ex);
                return SeedStatus.Failed;
            }
        }

        if (!hasData)
        {
            progress?.Report(new ProgressReport { GlobalPercent = 60, Message = "Mintaadatok beszúrása..." });
            await InsertSampleDataAsync(ctx, progress, ct);
            progress?.Report(new ProgressReport { GlobalPercent = 80, Message = "Mintaadatok kész." });
            return SeedStatus.Seeded;
        }

        progress?.Report(new ProgressReport { GlobalPercent = 90, Message = "Adatok vizsgálata..." });
        var onlySamples = await HasOnlySampleDataAsync(ctx, logService, ct);
        return onlySamples ? SeedStatus.OnlySampleData : SeedStatus.None;
    }

    private static async Task InsertSampleDataAsync(AppDbContext db, IProgress<ProgressReport>? progress, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var paymentId = Guid.NewGuid();
        var paymentId2 = Guid.NewGuid();
        progress?.Report(new ProgressReport { SubtaskPercent = 10, Message = "Fizetési módok..." });
        db.PaymentMethods.AddRange(
            new PaymentMethod
            {
                Id = paymentId,
                Name = SamplePayment,
                DueInDays = 0,
                IsArchived = false,
                CreatedAt = now,
                UpdatedAt = now
            },
            new PaymentMethod
            {
                Id = paymentId2,
                Name = SamplePayment2,
                DueInDays = 8,
                IsArchived = false,
                CreatedAt = now,
                UpdatedAt = now
            });

        var unitId = Guid.NewGuid();
        var unitId2 = Guid.NewGuid();
        progress?.Report(new ProgressReport { SubtaskPercent = 30, Message = "Mértékegységek..." });
        db.Units.AddRange(
            new Unit
            {
                Id = unitId,
                Code = "DB",
                Name = SampleUnit,
                IsArchived = false,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Unit
            {
                Id = unitId2,
                Code = "KG",
                Name = SampleUnit2,
                IsArchived = false,
                CreatedAt = now,
                UpdatedAt = now
            });

        var groupId = Guid.NewGuid();
        progress?.Report(new ProgressReport { SubtaskPercent = 50, Message = "Termékcsoportok..." });
        db.ProductGroups.Add(new ProductGroup { Id = groupId, Name = SampleGroup, CreatedAt = now, UpdatedAt = now });

        var taxId = Guid.NewGuid();
        var taxId2 = Guid.NewGuid();
        progress?.Report(new ProgressReport { SubtaskPercent = 70, Message = "ÁFA kulcsok..." });
        db.TaxRates.AddRange(
            new TaxRate
            {
                Id = taxId,
                Code = "A27",
                Name = SampleTax,
                Percentage = 27m,
                EffectiveFrom = new DateTime(2020, 1, 1),
                CreatedAt = now,
                UpdatedAt = now
            },
            new TaxRate
            {
                Id = taxId2,
                Code = "A5",
                Name = SampleTax2,
                Percentage = 5m,
                EffectiveFrom = new DateTime(2020, 1, 1),
                CreatedAt = now,
                UpdatedAt = now
            });

        progress?.Report(new ProgressReport { SubtaskPercent = 90, Message = "Szállító és termék..." });
        var supplier = new Supplier { Name = SampleSupplier, TaxId = "12345678-1-42", IsArchived = false, CreatedAt = now, UpdatedAt = now };
        var supplier2 = new Supplier { Name = SampleSupplier2, TaxId = "87654321-2-12", IsArchived = false, CreatedAt = now, UpdatedAt = now };
        db.Suppliers.AddRange(supplier, supplier2);
        var product = new Product
        {
            Name = SampleProduct,
            Net = 1000m,
            Gross = 1270m,
            TaxRateId = taxId,
            UnitId = unitId,
            ProductGroupId = groupId,
            IsArchived = false,
            CreatedAt = now,
            UpdatedAt = now
        };
        var product2 = new Product
        {
            Name = SampleProduct2,
            Net = 2000m,
            Gross = 2540m,
            TaxRateId = taxId,
            UnitId = unitId2,
            ProductGroupId = groupId,
            IsArchived = false,
            CreatedAt = now,
            UpdatedAt = now
        };
        db.Products.AddRange(product, product2);
        await db.SaveChangesAsync(ct);

        var invoice = new Invoice
        {
            Number = "SAMPLE-001",
            Date = DateOnly.FromDateTime(DateTime.Today),
            SupplierId = supplier.Id,
            PaymentMethodId = paymentId,
            IsGross = false,
            CreatedAt = now,
            UpdatedAt = now
        };
        db.Invoices.Add(invoice);
        await db.SaveChangesAsync(ct);

        db.InvoiceItems.Add(new InvoiceItem
        {
            InvoiceId = invoice.Id,
            ProductId = product.Id,
            TaxRateId = taxId,
            Description = SampleProduct,
            Quantity = 1,
            UnitPrice = 1000m,
            CreatedAt = now,
            UpdatedAt = now
        });
        await db.SaveChangesAsync(ct);
        progress?.Report(new ProgressReport { SubtaskPercent = 100, Message = "Mentve." });
    }

    private static async Task<bool> HasOnlySampleDataAsync(AppDbContext db, ILogService logService, CancellationToken ct)
    {
        async Task<bool> CheckAsync()
        {
            var supplierCount = await db.Suppliers.CountAsync(ct);
            var productCount = await db.Products.CountAsync(ct);
            var groupCount = await db.ProductGroups.CountAsync(ct);
            var taxCount = await db.TaxRates.CountAsync(ct);
            var paymentCount = await db.PaymentMethods.CountAsync(ct);
            var unitCount = await db.Units.CountAsync(ct);

            if (supplierCount != 2 || productCount != 2 || groupCount != 1 || taxCount != 2 || paymentCount != 2 || unitCount != 2)
                return false;

            var sampleMatch =
                await db.Suppliers.AnyAsync(s => s.Name == SampleSupplier, ct) &&
                await db.Suppliers.AnyAsync(s => s.Name == SampleSupplier2, ct) &&
                await db.Products.AnyAsync(p => p.Name == SampleProduct, ct) &&
                await db.Products.AnyAsync(p => p.Name == SampleProduct2, ct) &&
                await db.ProductGroups.AnyAsync(g => g.Name == SampleGroup, ct) &&
                await db.TaxRates.AnyAsync(t => t.Name == SampleTax, ct) &&
                await db.TaxRates.AnyAsync(t => t.Name == SampleTax2, ct) &&
                await db.PaymentMethods.AnyAsync(m => m.Name == SamplePayment, ct) &&
                await db.PaymentMethods.AnyAsync(m => m.Name == SamplePayment2, ct) &&
                await db.Units.AnyAsync(u => u.Name == SampleUnit, ct) &&
                await db.Units.AnyAsync(u => u.Name == SampleUnit2, ct);

            return sampleMatch;
        }
        try
        {
            return await CheckAsync();
        }
        catch (SqliteException ex)
        {
            await logService.LogError("Sample data check failed", ex);
            await DbInitializer.EnsureCreatedAndMigratedAsync(db, logService, ct);
            try
            {
                return await CheckAsync();
            }
            catch (Exception inner)
            {
                await logService.LogError("Sample data retry failed", inner);
                return false;
            }
        }
    }
}
