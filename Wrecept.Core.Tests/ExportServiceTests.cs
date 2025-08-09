using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.IO;

namespace Wrecept.Core.Tests;

public class ExportServiceTests
{
    private static AppDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task ExportImport_RoundTrip_Works()
    {
        await using var exportContext = CreateContext();
        var supplier = new Supplier { Name = "S1" };
        var product = new Product { Name = "P1", UnitPrice = 1m, VatRate = 0.1m };
        exportContext.Suppliers.Add(supplier);
        exportContext.Products.Add(product);
        await exportContext.SaveChangesAsync();

        var exportService = new ExportService(exportContext);
        var path = Path.GetTempFileName();
        await exportService.ExportAsync(path);

        await using var importContext = CreateContext();
        var importService = new ExportService(importContext);
        await importService.ImportAsync(path);

        Assert.Single(importContext.Products);
        Assert.Single(importContext.Suppliers);
    }
}
