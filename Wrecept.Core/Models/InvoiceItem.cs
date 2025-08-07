namespace Wrecept.Core.Models;

public class InvoiceItem
{
    private int _quantity;
    private decimal _unitPrice;
    private decimal _vatRate;

    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity cannot be negative.");
            _quantity = value;
            RecalculateTotals();
        }
    }

    public decimal UnitPrice
    {
        get => _unitPrice;
        set
        {
            if (value < 0m) throw new ArgumentOutOfRangeException(nameof(UnitPrice), "Unit price cannot be negative.");
            _unitPrice = value;
            RecalculateTotals();
        }
    }

    public decimal VatRate
    {
        get => _vatRate;
        set
        {
            if (value < 0m) throw new ArgumentOutOfRangeException(nameof(VatRate), "VAT rate cannot be negative.");
            _vatRate = value;
            RecalculateTotals();
        }
    }

    public decimal TotalNet { get; private set; }
    public decimal TotalVat { get; private set; }
    public decimal TotalGross { get; private set; }

    private void RecalculateTotals()
    {
        TotalNet = UnitPrice * Quantity;
        TotalVat = TotalNet * VatRate;
        TotalGross = TotalNet + TotalVat;
    }
}
