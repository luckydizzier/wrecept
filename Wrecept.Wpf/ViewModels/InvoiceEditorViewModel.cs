using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class InvoiceItemRowViewModel : ObservableObject
{
    [ObservableProperty]
    private string product = string.Empty;

    [ObservableProperty]
    private decimal quantity;

    [ObservableProperty]
    private decimal unitPrice;
}

public partial class InvoiceEditorViewModel : ObservableObject
{
    public ObservableCollection<InvoiceItemRowViewModel> Items { get; }

    public ObservableCollection<PaymentMethod> PaymentMethods { get; } = new();

    [ObservableProperty]
    private string supplier = string.Empty;

    [ObservableProperty]
    private string number = string.Empty;

    [ObservableProperty]
    private Guid paymentMethodId;

    [ObservableProperty]
    private bool isGross;

    private readonly IPaymentMethodService _paymentMethods;

    public InvoiceEditorViewModel(IPaymentMethodService paymentMethods)
    {
        _paymentMethods = paymentMethods;
        Items = new ObservableCollection<InvoiceItemRowViewModel>(
            Enumerable.Range(1, 3).Select(_ => new InvoiceItemRowViewModel()));
    }

    public async Task LoadAsync()
    {
        var methods = await _paymentMethods.GetActiveAsync();
        PaymentMethods.Clear();
        foreach (var m in methods)
            PaymentMethods.Add(m);
    }
}
