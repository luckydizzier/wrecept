using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Wrecept.Core.Models;
using System.IO;
using Wrecept.Core.Services;

namespace Wrecept.Storage.Data;

public static class DataSeeder
{
    private const string SampleSupplier = "Teszt Kft.";
    private const string SampleProduct = "Teszt termék";
    private const string SampleGroup = "Általános";
    private const string SampleTax = "ÁFA 27%";
    private const string SamplePayment = "Készpénz";

    public static async Task<SeedStatus> SeedAsync(string dbPath, ILogService logService, CancellationToken ct = default)
    {
        _ = File.Exists(dbPath);

        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        await using var ctx = new AppDbContext(opts);
        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, logService, ct);

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
            await InsertSampleDataAsync(ctx, ct);
            return SeedStatus.Seeded;
        }

        var onlySamples = await HasOnlySampleDataAsync(ctx, ct);
        return onlySamples ? SeedStatus.OnlySampleData : SeedStatus.None;
    }

    private static async Task InsertSampleDataAsync(AppDbContext db, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        db.PaymentMethods.Add(new PaymentMethod
        {
            Id = Guid.NewGuid(),
            Name = SamplePayment,
            DueInDays = 0,
            IsArchived = false,
            CreatedAt = now,
            UpdatedAt = now
        });
        db.ProductGroups.Add(new ProductGroup { Id = Guid.NewGuid(), Name = SampleGroup, CreatedAt = now, UpdatedAt = now });
        db.TaxRates.Add(new TaxRate
        {
            Id = Guid.NewGuid(),
            Name = SampleTax,
            Percentage = 27m,
            EffectiveFrom = new DateTime(2020, 1, 1),
            CreatedAt = now,
            UpdatedAt = now
        });
        db.Suppliers.Add(new Supplier { Name = SampleSupplier, TaxId = "12345678-1-42", CreatedAt = now, UpdatedAt = now });
        db.Products.Add(new Product { Name = SampleProduct, Net = 1000m, Gross = 1270m, CreatedAt = now, UpdatedAt = now });
        await db.SaveChangesAsync(ct);
    }

    private static async Task<bool> HasOnlySampleDataAsync(AppDbContext db, CancellationToken ct)
    {
        var supplierCount = await db.Suppliers.CountAsync(ct);
        var productCount = await db.Products.CountAsync(ct);
        var groupCount = await db.ProductGroups.CountAsync(ct);
        var taxCount = await db.TaxRates.CountAsync(ct);
        var paymentCount = await db.PaymentMethods.CountAsync(ct);

        if (supplierCount != 1 || productCount != 1 || groupCount != 1 || taxCount != 1 || paymentCount != 1)
            return false;

        var sampleMatch =
            await db.Suppliers.AnyAsync(s => s.Name == SampleSupplier, ct) &&
            await db.Products.AnyAsync(p => p.Name == SampleProduct, ct) &&
            await db.ProductGroups.AnyAsync(g => g.Name == SampleGroup, ct) &&
            await db.TaxRates.AnyAsync(t => t.Name == SampleTax, ct) &&
            await db.PaymentMethods.AnyAsync(m => m.Name == SamplePayment, ct);

        return sampleMatch;
    }
}
