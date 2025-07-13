using Microsoft.EntityFrameworkCore;
using InvoiceApp.Core.Models;

namespace InvoiceApp.Data.Data;

public class AppDbContext : DbContext
{
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<TaxRate> TaxRates => Set<TaxRate>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<ChangeLog> ChangeLogs => Set<ChangeLog>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxRate>()
            .HasIndex(t => new { t.EffectiveFrom, t.EffectiveTo, t.IsArchived });
        modelBuilder.Entity<TaxRate>()
            .HasIndex(t => new { t.Code, t.IsArchived });

        modelBuilder.Entity<Unit>()
            .HasIndex(u => new { u.Name, u.IsArchived });
        modelBuilder.Entity<Unit>()
            .HasIndex(u => new { u.Code, u.IsArchived });

        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.Date);
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.SupplierId);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name);
    }
}
