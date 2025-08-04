using System.Collections.Generic;

namespace Wrecept.WpfApp.Models;

public class Invoice
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public List<InvoiceItem> Items { get; set; } = new();
}
