using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;
using InvoiceApp.Data.Data;
using InvoiceApp.Data.Repositories;
using Xunit;

namespace InvoiceApp.Data.Tests;

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

    [Fact]
    public async Task UpdateHeaderAsync_UpdatesNumberAndFields()
    {
        var db = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".db");
        await using var ctx = await CreateContextAsync(db);
        var repo = new InvoiceRepository(ctx);
        var supplier = new Supplier { Name = "Test" };
        var payment1 = new PaymentMethod { Id = Guid.NewGuid(), Name = "Cash" };
        var payment2 = new PaymentMethod { Id = Guid.NewGuid(), Name = "Card" };
        ctx.Suppliers.Add(supplier);
        ctx.PaymentMethods.AddRange(payment1, payment2);
        var invoice = new Invoice
        {
            Number = "INV1",
            Date = new DateOnly(2024, 1, 1),
            Supplier = supplier,
            PaymentMethod = payment1,
            DueDate = new DateOnly(2024,1,10)
        };
        ctx.Invoices.Add(invoice);
        await ctx.SaveChangesAsync();

        await repo.UpdateHeaderAsync(invoice.Id, "INV2", new DateOnly(2024,2,2), new DateOnly(2024,2,12), supplier.Id, payment2.Id, true);
        var stored = await ctx.Invoices.FindAsync(invoice.Id);

        Assert.Equal("INV2", stored!.Number);
        Assert.Equal(new DateOnly(2024,2,2), stored.Date);
        Assert.Equal(new DateOnly(2024,2,12), stored.DueDate);
        Assert.Equal(payment2.Id, stored.PaymentMethodId);
        Assert.True(stored.IsGross);
    }
}
