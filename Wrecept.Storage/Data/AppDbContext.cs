using Microsoft.EntityFrameworkCore;
using Wrecept.Core.Models;

namespace Wrecept.Storage.Data;

// DbContext will be enabled in a later milestone
public class AppDbContext : DbContext
{
    // public DbSet<Invoice> Invoices => Set<Invoice>();
    // public DbSet<Product> Products => Set<Product>();
    // public DbSet<Supplier> Suppliers => Set<Supplier>();

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlite("Data Source=wrecept.db");
}
