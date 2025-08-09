using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wrecept.UI.ViewModels;

public class InvoiceItemVM : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private string _code = string.Empty;
    private string _description = string.Empty;
    private decimal _quantity = 1m;
    private string _unit = "db";
    private decimal _unitPrice;
    private decimal _taxRate;
    private decimal _lineNet;
    private decimal _lineVat;
    private decimal _lineGross;

    public string Code { get => _code; set { _code = value; OnPropertyChanged(); Validate(); } }
    public string Description { get => _description; set { _description = value; OnPropertyChanged(); Validate(); } }
    public decimal Quantity { get => _quantity; set { _quantity = value; OnPropertyChanged(); Recalculate(); Validate(); } }
    public string Unit { get => _unit; set { _unit = value; OnPropertyChanged(); } }
    public decimal UnitPrice { get => _unitPrice; set { _unitPrice = value; OnPropertyChanged(); Recalculate(); Validate(); } }
    public decimal TaxRate { get => _taxRate; set { _taxRate = value; OnPropertyChanged(); Recalculate(); } }

    public decimal LineNet { get => _lineNet; private set { _lineNet = value; OnPropertyChanged(); } }
    public decimal LineVat { get => _lineVat; private set { _lineVat = value; OnPropertyChanged(); } }
    public decimal LineGross { get => _lineGross; private set { _lineGross = value; OnPropertyChanged(); } }

    private void Recalculate()
    {
        LineNet = UnitPrice * Quantity;
        LineVat = LineNet * TaxRate;
        LineGross = LineNet + LineVat;
    }

    private readonly Dictionary<string, List<string>> _errors = new();
    public bool HasErrors => _errors.Any();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public IEnumerable GetErrors(string? propertyName)
        => propertyName != null && _errors.TryGetValue(propertyName, out var list) ? list : Enumerable.Empty<string>();

    private void Validate([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null) return;
        _errors.Remove(propertyName);
        var list = new List<string>();
        switch (propertyName)
        {
            case nameof(Code):
            case nameof(Description):
                if (string.IsNullOrWhiteSpace(propertyName == nameof(Code) ? Code : Description))
                    list.Add("Required");
                break;
            case nameof(Quantity):
                if (Quantity <= 0) list.Add("Quantity must be > 0");
                break;
            case nameof(UnitPrice):
                if (UnitPrice < 0) list.Add("Price must be >= 0");
                break;
        }
        if (list.Any()) _errors[propertyName] = list;
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
