using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Storage.Data;
using Wrecept.Storage.Repositories;

namespace Wrecept.Tests;

public class InvoiceServiceTests
{
    [Fact]
    public async Task CreateInvoice_WithoutItems_Fails()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("test")
            .Options;
        await using var ctx = new AppDbContext(options, "test.db");
        var repo = new InvoiceRepository(ctx);
        var service = new InvoiceService(repo);

        var invoice = new Invoice { Number = "A-1", Date = DateOnly.FromDateTime(DateTime.Today) };
        var result = await service.CreateAsync(invoice);
        Assert.False(result);
    }
}
