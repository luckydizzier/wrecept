using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace InvoiceApp.MAUI.ViewModels;

public partial class SeedOptionsViewModel : ObservableObject
{
    [ObservableProperty]
    private int supplierCount = 20;

    [ObservableProperty]
    private int productCount = 500;

    [ObservableProperty]
    private int invoiceCount = 100;

    [ObservableProperty]
    private int minItemsPerInvoice = 5;

    [ObservableProperty]
    private int maxItemsPerInvoice = 60;

    public IRelayCommand OkCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public event Action<bool>? DialogResult;

    public SeedOptionsViewModel()
    {
        OkCommand = new RelayCommand(() => DialogResult?.Invoke(true));
        CancelCommand = new RelayCommand(() => DialogResult?.Invoke(false));
    }
}
