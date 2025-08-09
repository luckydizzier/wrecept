using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Data;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Core.Tests;

public class RepositoryTests
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
    public async Task AddAsync_RollsBack_OnError()
    {
        await using var context = CreateContext();
        var repo = new Repository<Invoice>(context);
        var invoice = new Invoice { SupplierId = 999 }; // invalid FK
        await Assert.ThrowsAsync<DbUpdateException>(() => repo.AddAsync(invoice));
        Assert.Empty(context.Invoices);
    }
}
