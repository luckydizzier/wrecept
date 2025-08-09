using System;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Core.Tests;

public class AnalyticsServiceTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;
        var ctx = new AppDbContext(options);
        ctx.Database.OpenConnection();
        ctx.Database.EnsureCreated();
        return ctx;
    }

    [Fact]
    public async Task GetTopProductsAsync_ReturnsOrderedProducts()
    {
        using var ctx = CreateContext();
        var supplier = new Supplier { Name = "Supp" };
        var coffee = new Product { Name = "Coffee", UnitPrice = 10m, VatRate = 0.27m };
        var tea = new Product { Name = "Tea", UnitPrice = 5m, VatRate = 0.27m };
        ctx.AddRange(supplier, coffee, tea);
        ctx.SaveChanges();

        var invoice = new Invoice { SupplierId = supplier.Id, Supplier = supplier, Date = DateTime.UtcNow };
        var item1 = new InvoiceItem { Invoice = invoice, Product = coffee, Quantity = 2, UnitPrice = 10m, VatRate = 0.27m };
        var item2 = new InvoiceItem { Invoice = invoice, Product = tea, Quantity = 1, UnitPrice = 5m, VatRate = 0.27m };
        invoice.Items.Add(item1);
        invoice.Items.Add(item2);
        invoice.RecalculateTotals();
        ctx.Add(invoice);
        ctx.SaveChanges();

        var svc = new AnalyticsService(ctx);
        var top = await svc.GetTopProductsAsync(1);
        Assert.Single(top);
        Assert.Equal("Coffee", top[0].ProductName);
    }
}
