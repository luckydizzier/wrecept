using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;
using Xunit;

namespace Wrecept.Storage.Tests;

public class InvoiceRepositoryTests
{
    private class DummyLogService : ILogService
    {
        public Task LogError(string message, Exception ex) => Task.CompletedTask;
    }

    private static async Task<AppDbContext> CreateContextAsync(string db)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={db}")
            .Options;
        var ctx = new AppDbContext(opts);
        await DbInitializer.EnsureCreatedAndMigratedAsync(ctx, new DummyLogService());
        return ctx;
    }

    [Fact]
    public async Task AddAsync_PersistsInvoice()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new InvoiceRepository(ctx);
        var supplier = new Supplier { Name = "Test" };
        var payment = new PaymentMethod { Id = Guid.NewGuid(), Name = "Cash" };
        ctx.Suppliers.Add(supplier);
        ctx.PaymentMethods.Add(payment);
        await ctx.SaveChangesAsync();
        var invoice = new Invoice
        {
            Number = "INV1",
            Date = new DateOnly(2024, 1, 1),
            SupplierId = supplier.Id,
            PaymentMethodId = payment.Id,
            DueDate = new DateOnly(2024, 1, 10)
        };

        var id = await repo.AddAsync(invoice);

        Assert.True(id > 0);
        Assert.Single(ctx.Invoices);
    }

    [Fact]
    public async Task SetArchivedAsync_FlagsInvoice()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid()+".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new InvoiceRepository(ctx);
        var supplier = new Supplier { Name = "Test" };
        var payment = new PaymentMethod { Id = Guid.NewGuid(), Name = "Cash" };
        ctx.Suppliers.Add(supplier);
        ctx.PaymentMethods.Add(payment);
        var invoice = new Invoice
        {
            Number = "INV1",
            Date = new DateOnly(2024, 1, 1),
            Supplier = supplier,
            PaymentMethod = payment,
            DueDate = new DateOnly(2024,1,10)
        };
        ctx.Invoices.Add(invoice);
        await ctx.SaveChangesAsync();

        await repo.SetArchivedAsync(invoice.Id, true);
        var stored = await ctx.Invoices.FindAsync(invoice.Id);

        Assert.True(stored!.IsArchived);
    }
}
