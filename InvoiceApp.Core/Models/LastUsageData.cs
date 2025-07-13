namespace InvoiceApp.Core.Models;

public class LastUsageData
{
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Guid TaxRateId { get; set; }
}

