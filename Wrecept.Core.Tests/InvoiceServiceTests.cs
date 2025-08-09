using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;

namespace Wrecept.Core.Tests;

public class InvoiceServiceTests
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
    public async Task AddInvoiceAsync_CalculatesTotals()
    {
        await using var context = CreateContext();
        context.Suppliers.Add(new Supplier { Name = "Test" });
        var product = new Product { Name = "P1", UnitPrice = 10m, VatRate = 0.27m };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var repo = new Repository<Invoice>(context);
        var service = new InvoiceService(repo);

        var invoice = new Invoice { SupplierId = 1 };
        invoice.Items.Add(new InvoiceItem { ProductId = product.Id, Quantity = 2, UnitPrice = product.UnitPrice, VatRate = product.VatRate });

        await service.AddInvoiceAsync(invoice);

        var saved = await context.Invoices.Include(i => i.Items).FirstAsync();
        Assert.Equal(20m, saved.TotalNet);
        Assert.Equal(5.4m, saved.TotalVat);
        Assert.Equal(25.4m, saved.TotalGross);
    }
}
