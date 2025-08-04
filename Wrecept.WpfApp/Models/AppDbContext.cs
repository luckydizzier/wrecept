using Microsoft.EntityFrameworkCore;

namespace Wrecept.WpfApp.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();
}

public class Invoice
{
    public int Id { get; set; }
}
