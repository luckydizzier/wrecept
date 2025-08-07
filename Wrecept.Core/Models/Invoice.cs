using System.Collections.Generic;
using System.Linq;

namespace Wrecept.Core.Models;

public class Invoice
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public List<InvoiceItem> Items { get; set; } = new();

    public decimal TotalNet { get; private set; }
    public decimal TotalVat { get; private set; }
    public decimal TotalGross { get; private set; }

    public void RecalculateTotals()
    {
        TotalNet = Items.Sum(i => i.TotalNet);
        TotalVat = Items.Sum(i => i.TotalVat);
        TotalGross = Items.Sum(i => i.TotalGross);
    }
}
