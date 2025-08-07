using System.Collections.Generic;
using Wrecept.Core.Data;
using Wrecept.Core.Models;

namespace Wrecept.Core.Services;

public class DemoDataService : IDemoDataService
{
    private readonly AppDbContext _dbContext;

    public DemoDataService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        var supplier = new Supplier { Name = "Demo Supplier" };
        var product = new Product { Name = "Demo Product" };

        var invoice = new Invoice
        {
            Supplier = supplier,
            Items = new List<InvoiceItem>
            {
                new InvoiceItem { Product = product, Quantity = 1 }
            }
        };

        _dbContext.Invoices.Add(invoice);
        await _dbContext.SaveChangesAsync();
    }
}
