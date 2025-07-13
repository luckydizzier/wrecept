using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InvoiceApp.Data.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var dbPath = args.Length > 0 ? args[0] : "app.db";
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .AddInterceptors(new WalPragmaInterceptor())
            .Options;
        return new AppDbContext(options);
    }
}
