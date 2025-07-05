using System;
using System.IO;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Wpf.Services;
using Xunit;

namespace Wrecept.Tests;

public class PdfInvoiceExporterTests
{
    [Fact]
    public async Task SavePdfAsync_CreatesFile()
    {
        var invoice = new Invoice
        {
            Number = "TEST-1",
            Date = DateOnly.FromDateTime(DateTime.Today),
            Supplier = new Supplier { Name = "Teszt" },
            PaymentMethod = new PaymentMethod { Name = "Készpénz" },
            Items = { new InvoiceItem { Quantity = 1, UnitPrice = 100, Product = new Product { Name = "Termék" }, TaxRate = new TaxRate { Name = "27%" } } }
        };
        var exporter = new PdfInvoiceExporter();
        var file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");
        await exporter.SavePdfAsync(invoice, file);
        Assert.True(File.Exists(file));
        File.Delete(file);
    }
}
