namespace Wrecept.Core.Models;

public class Product
{
    private decimal _unitPrice;
    private decimal _vatRate;

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public decimal UnitPrice
    {
        get => _unitPrice;
        set
        {
            if (value < 0m) throw new ArgumentOutOfRangeException(nameof(UnitPrice), "Unit price cannot be negative.");
            _unitPrice = value;
        }
    }

    public decimal VatRate
    {
        get => _vatRate;
        set
        {
            if (value < 0m) throw new ArgumentOutOfRangeException(nameof(VatRate), "VAT rate cannot be negative.");
            _vatRate = value;
        }
    }
}
