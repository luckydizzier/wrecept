using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;

namespace Wrecept.Storage.Data;

public class AppDbContext : DbContext
{
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    private readonly string _dbPath;

    public AppDbContext(string dbPath)
    {
        _dbPath = dbPath;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, string dbPath)
        : base(options)
    {
        _dbPath = dbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={_dbPath}");
}
