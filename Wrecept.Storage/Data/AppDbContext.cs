using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;
using Wrecept.Core.Entities;

namespace Wrecept.Storage.Data;

public class AppDbContext : DbContext
{
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<TaxRate> TaxRates => Set<TaxRate>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxRate>()
            .HasIndex(t => new { t.EffectiveFrom, t.EffectiveTo, t.IsArchived });
    }
}
