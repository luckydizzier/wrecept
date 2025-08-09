using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Core.Tests;

public class ProductLookupServiceTests
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
    public async Task SearchAsync_IsCaseInsensitive()
    {
        using var ctx = CreateContext();
        ctx.Products.AddRange(
            new Product { Name = "Coffee" },
            new Product { Name = "Tea" }
        );
        ctx.SaveChanges();

        var svc = new ProductLookupService(ctx);
        var results = await svc.SearchAsync("coffee");
        Assert.Single(results);
        Assert.Equal("Coffee", results[0].Name);
    }
}
