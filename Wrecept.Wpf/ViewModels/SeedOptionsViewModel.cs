using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Wrecept.Wpf.ViewModels;

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

    public IRelayCommand<Window?> OkCommand { get; }
    public IRelayCommand<Window?> CancelCommand { get; }

    public SeedOptionsViewModel()
    {
        OkCommand = new RelayCommand<Window?>(w => { if (w != null) { w.DialogResult = true; w.Close(); } });
        CancelCommand = new RelayCommand<Window?>(w => { if (w != null) { w.DialogResult = false; w.Close(); } });
    }
}
