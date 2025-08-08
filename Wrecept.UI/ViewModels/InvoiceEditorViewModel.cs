using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.UI.ViewModels;

public class InvoiceEditorViewModel : INotifyPropertyChanged
{
    private readonly IInvoiceService _invoiceService;

    public ObservableCollection<InvoiceItem> Items { get; } = new();
    private InvoiceItem? _selectedItem;
    public InvoiceItem? SelectedItem
    {
        get => _selectedItem;
        set { _selectedItem = value; OnPropertyChanged(); }
    }

    public Invoice Invoice { get; } = new();

    public ICommand AddItemCommand { get; }
    public ICommand DeleteItemCommand { get; }
    public ICommand SaveCommand { get; }

    public InvoiceEditorViewModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        AddItemCommand = new RelayCommand(_ => AddItem());
        DeleteItemCommand = new RelayCommand(_ => DeleteItem(), _ => SelectedItem != null);
        SaveCommand = new RelayCommand(_ => SaveInvoice());
    }

    private void AddItem()
    {
        var item = new InvoiceItem();
        Items.Add(item);
        SelectedItem = item;
    }

    private void DeleteItem()
    {
        if (SelectedItem != null)
        {
            var confirm = _dialogService.ShowConfirmation(
                Resources.BiztosanTorliATetelt,
                "Megerősítés");
            if (!confirm) return;

            Items.Remove(SelectedItem);
            SelectedItem = null;
        }
    }

    private async void SaveInvoice()
    {
        var confirm = MessageBox.Show(
            "Biztosan menti a számlát?",
            "Megerősítés",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes) return;

        Invoice.Items = Items.ToList();
        Invoice.RecalculateTotals();
        try
        {
            await _invoiceService.AddInvoiceAsync(Invoice);
            MessageBox.Show("Számla elmentve.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Mentési hiba: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
