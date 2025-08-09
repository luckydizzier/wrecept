using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.UI.ViewModels;

public class ListsViewModel : INotifyPropertyChanged
{
    private readonly IInvoiceService _invoiceService;
    public ObservableCollection<Invoice> Invoices { get; } = new();
    private Invoice? _selectedInvoice;
    public Invoice? SelectedInvoice
    {
        get => _selectedInvoice;
        set { _selectedInvoice = value; OnPropertyChanged(); }
    }

    public ICommand RefreshCommand { get; }

    public ListsViewModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        RefreshCommand = new AsyncRelayCommand(_ => RefreshAsync());
        _ = RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        var items = await _invoiceService.GetInvoicesAsync();
        Invoices.Clear();
        foreach (var i in items)
        {
            i.RecalculateTotals();
            Invoices.Add(i);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
