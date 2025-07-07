using System;
using System.IO;
using System.Diagnostics;
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

    [Fact]
    public async Task PrintAsync_CreatesFile_UsesPrintVerb()
    {
        var invoice = new Invoice { Number = "P1", Date = DateOnly.FromDateTime(DateTime.Today) };
        var exporter = new PdfInvoiceExporter();
        ProcessStartInfo? captured = null;
        PdfInvoiceExporter.ProcessStarter = psi => { captured = psi; return null; };

        await exporter.PrintAsync(invoice);

        Assert.NotNull(captured);
        Assert.Equal("print", captured!.Verb);
        Assert.True(File.Exists(captured!.FileName));
        File.Delete(captured!.FileName);
        PdfInvoiceExporter.ProcessStarter = Process.Start;
    }

    [Fact]
    public async Task SavePdfAsync_NullInvoice_Throws()
    {
        var exporter = new PdfInvoiceExporter();
        await Assert.ThrowsAsync<ArgumentNullException>(() => exporter.SavePdfAsync(null!, "a"));
    }

    [Fact]
    public async Task SavePdfAsync_NullPath_Throws()
    {
        var exporter = new PdfInvoiceExporter();
        await Assert.ThrowsAsync<ArgumentNullException>(() => exporter.SavePdfAsync(new Invoice(), null!));
    }

    [Fact]
    public async Task PrintAsync_NullInvoice_Throws()
    {
        var exporter = new PdfInvoiceExporter();
        await Assert.ThrowsAsync<ArgumentNullException>(() => exporter.PrintAsync(null!));
    }
}
