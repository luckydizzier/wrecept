using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Data.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
