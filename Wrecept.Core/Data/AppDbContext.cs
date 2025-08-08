using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Wrecept.Core.Models;

namespace Wrecept.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SuggestionTerm> SuggestionTerms => Set<SuggestionTerm>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("wrecept.json", optional: false)
                .Build();

            var dbPath = configuration["DatabasePath"];
            if (string.IsNullOrWhiteSpace(dbPath))
            {
                throw new InvalidOperationException("DatabasePath configuration value is missing or empty.");
            }
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Items)
            .WithOne(ii => ii.Invoice)
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Supplier)
            .WithMany()
            .HasForeignKey(i => i.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalNet).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalVat).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalGross).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.Date);
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.SupplierId);

        modelBuilder.Entity<InvoiceItem>()
            .HasOne(ii => ii.Product)
            .WithMany()
            .HasForeignKey(ii => ii.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<InvoiceItem>()
            .Property(ii => ii.UnitPrice).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<InvoiceItem>()
            .Property(ii => ii.VatRate).HasColumnType("decimal(5,4)");
        modelBuilder.Entity<InvoiceItem>()
            .Property(ii => ii.TotalNet).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<InvoiceItem>()
            .Property(ii => ii.TotalVat).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<InvoiceItem>()
            .Property(ii => ii.TotalGross).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Product>()
            .Property(p => p.VatRate).HasColumnType("decimal(5,4)");

        modelBuilder.Entity<SuggestionTerm>()
            .Property(s => s.Term)
            .UseCollation("NOCASE");
        modelBuilder.Entity<SuggestionTerm>()
            .HasIndex(s => s.Term)
            .IsUnique();
        modelBuilder.Entity<SuggestionTerm>()
            .HasIndex(s => s.LastUsedUtc);

        base.OnModelCreating(modelBuilder);
    }
}
