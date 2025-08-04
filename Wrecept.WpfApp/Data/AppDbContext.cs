using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Wrecept.WpfApp.Models;

namespace Wrecept.WpfApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("wrecept.json", optional: false)
                .Build();

            var dbPath = configuration["DatabasePath"];
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
