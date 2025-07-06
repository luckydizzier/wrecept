using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Wrecept.Wpf.Services;

public class PdfInvoiceExporter : IInvoiceExportService
{
    static PdfInvoiceExporter()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public Task SavePdfAsync(Invoice invoice, string filePath, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        ArgumentNullException.ThrowIfNull(filePath);
        var document = CreateDocument(invoice);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        document.GeneratePdf(filePath);
        return Task.CompletedTask;
    }

    public async Task PrintAsync(Invoice invoice, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        var temp = Path.Combine(Path.GetTempPath(), $"{invoice.Number}_{Guid.NewGuid():N}.pdf");
        await SavePdfAsync(invoice, temp, ct);
        var psi = new ProcessStartInfo(temp)
        {
            UseShellExecute = true,
            Verb = "print"
        };
        Process.Start(psi);
    }

    private static Document CreateDocument(Invoice invoice)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Header().Text($"Számla: {invoice.Number}").SemiBold().FontSize(18);
                page.Content().Element(c => ComposeTable(c, invoice));
                page.Footer().Text($"Dátum: {invoice.Date:yyyy-MM-dd}");
            });
        });
    }

    private static void ComposeTable(IContainer container, Invoice invoice)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(2);
                columns.RelativeColumn(6);
                columns.RelativeColumn(2);
                columns.RelativeColumn(2);
            });

            table.Header(header =>
            {
                header.Cell().Text("Menny.").SemiBold();
                header.Cell().Text("Termék").SemiBold();
                header.Cell().AlignRight().Text("Egysár").SemiBold();
                header.Cell().AlignRight().Text("ÁFA").SemiBold();
            });

            foreach (var item in invoice.Items)
            {
                table.Cell().Text(item.Quantity.ToString());
                table.Cell().Text(item.Product?.Name ?? string.Empty);
                table.Cell().AlignRight().Text(item.UnitPrice.ToString("0.00"));
                table.Cell().AlignRight().Text(item.TaxRate?.Name ?? string.Empty);
            }
        });
    }
}
