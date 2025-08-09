using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.UI.Services;

namespace Wrecept.UI.ViewModels;

public class InvoiceViewModel : INotifyPropertyChanged
{
    private readonly IInvoiceService _invoiceService;
    private readonly IProductLookupService _productLookupService;
    private readonly ITaxService _taxService;
    private readonly ISettingsService _settingsService;
    private readonly IMessageService _messageService;

    private readonly ObservableCollection<InvoiceItemVM> _items = new();
    public ObservableCollection<InvoiceItemVM> Items => _items;

    public ObservableCollection<decimal> TaxRates { get; } = new();
    public ObservableCollection<Product> ProductResults { get; } = new();

    private InvoiceItemVM? _selectedItem;
    public InvoiceItemVM? SelectedItem
    {
        get => _selectedItem;
        set { _selectedItem = value; OnPropertyChanged(); }
    }

    private Product? _selectedProduct;
    public Product? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            if (_selectedProduct != value)
            {
                _selectedProduct = value;
                OnPropertyChanged();
                if (value != null && SelectedItem != null)
                {
                    SelectedItem.Code = value.Id.ToString();
                    SelectedItem.Description = value.Name;
                    SelectedItem.UnitPrice = value.UnitPrice;
                    SelectedItem.TaxRate = value.VatRate;
                    IsProductSearchOpen = false;
                    ProductResults.Clear();
                    RecalculateTotals();
                }
            }
        }
    }

    public string Customer { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; } = DateTime.Today;
    public ObservableCollection<string> PaymentMethods { get; } = new() { "Cash", "Card" };
    private string? _selectedPaymentMethod;
    public string? SelectedPaymentMethod { get => _selectedPaymentMethod; set { _selectedPaymentMethod = value; OnPropertyChanged(); } }

    private bool _isProductSearchOpen;
    public bool IsProductSearchOpen
    {
        get => _isProductSearchOpen;
        private set { _isProductSearchOpen = value; OnPropertyChanged(); }
    }

    private CancellationTokenSource? _searchCts;

    private decimal _totalNet;
    public decimal TotalNet { get => _totalNet; private set { _totalNet = value; OnPropertyChanged(); } }
    private decimal _totalVat;
    public decimal TotalVat { get => _totalVat; private set { _totalVat = value; OnPropertyChanged(); } }
    private decimal _totalGross;
    public decimal TotalGross { get => _totalGross; private set { _totalGross = value; OnPropertyChanged(); } }

    public ObservableCollection<VatTotal> VatTotals { get; } = new();

    private string _statusMessage = "Ready";
    public string StatusMessage { get => _statusMessage; private set { _statusMessage = value; OnPropertyChanged(); } }

    private bool _isBusy;
    public bool IsBusy { get => _isBusy; private set { _isBusy = value; OnPropertyChanged(); } }

    public RelayCommand AddItemCommand { get; }
    public RelayCommand RemoveItemCommand { get; }
    public AsyncRelayCommand SaveInvoiceCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand ShowVatBreakdownCommand { get; }
    public RelayCommand ApplyDiscountCommand { get; }

    public InvoiceViewModel(IInvoiceService invoiceService,
                            IProductLookupService productLookupService,
                            ITaxService taxService,
                            ISettingsService settingsService,
                            IMessageService messageService)
    {
        _invoiceService = invoiceService;
        _productLookupService = productLookupService;
        _taxService = taxService;
        _settingsService = settingsService;
        _messageService = messageService;

        AddItemCommand = new RelayCommand(_ => AddItem());
        RemoveItemCommand = new RelayCommand(_ => RemoveItem(), _ => SelectedItem != null);
        SaveInvoiceCommand = new AsyncRelayCommand(_ => SaveAsync(), _ => CanSave());
        CancelCommand = new RelayCommand(_ => Cancel());
        ShowVatBreakdownCommand = new RelayCommand(_ => ShowVatBreakdown());
        ApplyDiscountCommand = new RelayCommand(p => ApplyDiscount(Convert.ToDecimal(p ?? 0m)));

        _items.CollectionChanged += ItemsChanged;
    }

    public async Task InitializeAsync()
    {
        var settings = await _settingsService.LoadAsync();
        if (string.IsNullOrWhiteSpace(settings.Theme))
            settings.Theme = "Light";
        var rates = await _taxService.GetRatesAsync();
        foreach (var r in rates) TaxRates.Add(r);
        StatusMessage = "Ready";
    }

    private void ItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (InvoiceItemVM item in e.NewItems)
            {
                item.PropertyChanged += ItemChanged;
                item.ErrorsChanged += (_, __) => UpdateValidation();
            }
        }
        if (e.OldItems != null)
        {
            foreach (InvoiceItemVM item in e.OldItems)
            {
                item.PropertyChanged -= ItemChanged;
            }
        }
        RecalculateTotals();
        UpdateValidation();
    }

    private void ItemChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(InvoiceItemVM.Code) || e.PropertyName == nameof(InvoiceItemVM.Description))
        {
            var term = e.PropertyName == nameof(InvoiceItemVM.Code) ? SelectedItem?.Code : SelectedItem?.Description;
            if (term != null)
                _ = OpenProductSearchAsync(term);
        }
        RecalculateTotals();
        UpdateValidation();
    }

    public async Task OpenProductSearchAsync(string term)
    {
        _searchCts?.Cancel();
        if (string.IsNullOrWhiteSpace(term))
        {
            ProductResults.Clear();
            IsProductSearchOpen = false;
            return;
        }

        var cts = _searchCts = new CancellationTokenSource();
        try
        {
            await Task.Delay(300, cts.Token);
            var results = await _productLookupService.SearchAsync(term);
            ProductResults.Clear();
            foreach (var p in results) ProductResults.Add(p);
            IsProductSearchOpen = ProductResults.Any();
        }
        catch (TaskCanceledException) { }
    }

    private void AddItem()
    {
        var item = new InvoiceItemVM();
        _items.Add(item);
        SelectedItem = item;
    }

    private void RemoveItem()
    {
        if (SelectedItem == null) return;
        _items.Remove(SelectedItem);
        SelectedItem = null;
    }

    private bool CanSave() => Items.Any() && Items.All(i => !i.HasErrors);

    private async Task SaveAsync()
    {
        if (!CanSave())
        {
            StatusMessage = "Validation errors";
            return;
        }
        if (!_messageService.Confirm("Mented a számlát?", "Megerősítés")) return;

        IsBusy = true;
        try
        {
            var invoice = new Invoice
            {
                Items = Items.Select(i => new InvoiceItem
                {
                    Quantity = (int)i.Quantity,
                    UnitPrice = i.UnitPrice,
                    VatRate = i.TaxRate
                }).ToList()
            };
            await _invoiceService.AddInvoiceAsync(invoice);
            StatusMessage = "Ready";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void Cancel()
    {
        _items.Clear();
        RecalculateTotals();
        StatusMessage = "Cancelled";
    }

    private void ShowVatBreakdown()
    {
        var msg = string.Join("\n", VatTotals.Select(v => $"{v.Rate:P0}: {v.Vat:N2}"));
        _messageService.Show(msg.Length == 0 ? "Nincs ÁFA" : msg, "ÁFA összesítés");
    }

    private void ApplyDiscount(decimal percent)
    {
        foreach (var item in Items)
        {
            item.UnitPrice -= item.UnitPrice * percent / 100m;
        }
        RecalculateTotals();
    }

    public void RecalculateTotals()
    {
        TotalNet = Items.Sum(i => i.LineNet);
        TotalVat = Items.Sum(i => i.LineVat);
        TotalGross = TotalNet + TotalVat;
        VatTotals.Clear();
        foreach (var g in Items.GroupBy(i => i.TaxRate))
        {
            VatTotals.Add(new VatTotal(g.Key, g.Sum(x => x.LineVat)));
        }
    }

    private void UpdateValidation()
    {
        var errors = Items.SelectMany(i => i.HasErrors ? new[] { i } : Array.Empty<InvoiceItemVM>()).Count();
        StatusMessage = errors > 0 ? $"{errors} validation error(s)" : "Ready";
        SaveInvoiceCommand.RaiseCanExecuteChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
