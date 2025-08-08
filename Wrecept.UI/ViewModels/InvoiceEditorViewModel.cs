using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

    public ObservableCollection<string> Suggestions { get; } = new();
    private string _searchTerm = string.Empty;
    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (_searchTerm != value)
            {
                _searchTerm = value;
                OnPropertyChanged();
                UpdateSuggestions();
            }
        }
    }

    private string? _selectedSuggestion;
    public string? SelectedSuggestion
    {
        get => _selectedSuggestion;
        set { _selectedSuggestion = value; OnPropertyChanged(); }
    }

    public bool HasSuggestions => _hasSuggestions;

    public ICommand AddItemCommand { get; }
    public ICommand DeleteItemCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand SelectSuggestionCommand { get; }

    public InvoiceEditorViewModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        AddItemCommand = new RelayCommand(_ => AddItem());
        DeleteItemCommand = new RelayCommand(_ => DeleteItem(), _ => SelectedItem != null);
        SaveCommand = new RelayCommand(_ => SaveInvoice());
        SelectSuggestionCommand = new RelayCommand(p => SelectSuggestion(p as string));
        Suggestions.CollectionChanged += (_, __) => OnPropertyChanged(nameof(HasSuggestions));
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
            var messageObj = Application.Current.TryFindResource("BiztosanTorliATetelt");
            var captionObj = Application.Current.TryFindResource("Confirmation");
            var message = messageObj as string ?? "Biztosan törli a tételt?";
            var caption = captionObj as string ?? "Megerősítés";
            var confirm = MessageBox.Show(
                message,
                caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            Items.Remove(SelectedItem);
            SelectedItem = null;
        }
    }

    private async void SaveInvoice()
    {
        string saveMsg = Application.Current.TryFindResource("ConfirmSaveInvoice") as string
                          ?? "Biztosan menti a számlát?";
        string caption = Application.Current.TryFindResource("Confirmation") as string
                          ?? "Megerősítés";
        var confirm = MessageBox.Show(
            saveMsg,
            caption,
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

    private void SelectSuggestion(string? suggestion)
    {
        if (suggestion == null) return;
        SearchTerm = suggestion;
        Suggestions.Clear();
    }

    private void UpdateSuggestions()
    {
        Suggestions.Clear();
        if (string.IsNullOrWhiteSpace(SearchTerm)) return;

        // Suggest items from existing invoice data that match the search term
        var matchingItems = Items
            .Where(item => !string.IsNullOrWhiteSpace(item.Name) && item.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            .Select(item => item.Name)
            .Distinct()
            .ToList();
        foreach (var suggestion in matchingItems)
        {
            Suggestions.Add(suggestion);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
