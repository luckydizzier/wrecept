using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;

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

    [ObservableProperty]
    private string supplier = string.Empty;

    [ObservableProperty]
    private string number = string.Empty;

    public InvoiceEditorViewModel()
    {
        Items = new ObservableCollection<InvoiceItemRowViewModel>(
            Enumerable.Range(1, 3).Select(_ => new InvoiceItemRowViewModel()));
    }
}
