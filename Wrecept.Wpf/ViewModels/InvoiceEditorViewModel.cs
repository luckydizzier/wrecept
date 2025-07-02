using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Utilities;
using Wrecept.Wpf.Resources;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class InvoiceItemRowViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;

    public InvoiceItemRowViewModel(InvoiceEditorViewModel parent)
    {
        _parent = parent;
    }

    [ObservableProperty]
    private string product = string.Empty;

    partial void OnProductChanged(string value)
        => _ = _parent.CheckProductAsync(this, value);

    [ObservableProperty]
    private decimal quantity;

    [ObservableProperty]
    private decimal unitPrice;

    [ObservableProperty]
    private Guid taxRateId;

    [ObservableProperty]
    private string taxRateName = string.Empty;

    partial void OnTaxRateIdChanged(Guid value)
        => TaxRateName = _parent.TaxRates.FirstOrDefault(t => t.Id == value)?.Name ?? string.Empty;

    [ObservableProperty]
    private Guid unitId;

    [ObservableProperty]
    private string unitName = string.Empty;

    partial void OnUnitIdChanged(Guid value)
        => UnitName = _parent.Units.FirstOrDefault(u => u.Id == value)?.Name ?? string.Empty;

    [ObservableProperty]
    private string productGroup = string.Empty;

    [ObservableProperty]
    private bool isEditable = true;

    [ObservableProperty]
    private bool hasError;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isFirstRow;

    [ObservableProperty]
    private bool isAutofilled;
}

public partial class VatSummaryRowViewModel : ObservableObject
{
    [ObservableProperty]
    private string rate = string.Empty;

    [ObservableProperty]
    private decimal net;

    [ObservableProperty]
    private decimal vat;

    [ObservableProperty]
    private decimal gross;
}

public partial class InvoiceEditorViewModel : ObservableObject
{
    public ObservableCollection<InvoiceItemRowViewModel> Items { get; }

    public InvoiceLookupViewModel Lookup { get; }

    public ObservableCollection<PaymentMethod> PaymentMethods { get; } = new();
    public ObservableCollection<TaxRate> TaxRates { get; } = new();
    public ObservableCollection<Supplier> Suppliers { get; } = new();
    public ObservableCollection<Product> Products { get; } = new();
    public ObservableCollection<Unit> Units { get; } = new();

    private static int StepPercent(int step, int total) => step * 100 / total;

    private static async Task LoadCollectionAsync<T>(ObservableCollection<T> target, Task<List<T>> loadTask, string message, int step, int totalSteps, IProgress<ProgressReport>? progress)
    {
        progress?.Report(new ProgressReport { GlobalPercent = StepPercent(step, totalSteps), SubtaskPercent = 0, Message = message });
        var items = await loadTask;
        target.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            target.Add(items[i]);
            progress?.Report(new ProgressReport
            {
                GlobalPercent = StepPercent(step, totalSteps),
                SubtaskPercent = (i + 1) * 100 / items.Count,
                Message = $"{message} {i + 1}/{items.Count}"
            });
        }
    }

    [ObservableProperty]
    private string supplier = string.Empty;
partial void OnSupplierChanged(string value) => UpdateSupplierId(value);

    [ObservableProperty]
    private int supplierId;

    [ObservableProperty]
    private DateTime? invoiceDate;

    [ObservableProperty]
    private string number = string.Empty;

    [ObservableProperty]
    private bool isNew = true;

    [ObservableProperty]
    private Guid paymentMethodId;

    [ObservableProperty]
    private bool isGross;

    [ObservableProperty]
    private bool isArchived;

    public bool IsEditable => !IsArchived;

    partial void OnIsArchivedChanged(bool value) => OnPropertyChanged(nameof(IsEditable));

    [ObservableProperty]
    private decimal netTotal;

    [ObservableProperty]
    private decimal vatTotal;

    [ObservableProperty]
    private decimal grossTotal;

    public ObservableCollection<VatSummaryRowViewModel> VatSummaries { get; } = new();

    [ObservableProperty]
    private string amountInWords = string.Empty;

    private readonly IPaymentMethodService _paymentMethods;
    private readonly ITaxRateService _taxRates;
    private readonly ISupplierService _suppliers;
    private readonly IProductService _productsService;
    private readonly IUnitService _unitsService;
    private readonly IInvoiceService _invoiceService;
    private readonly ILogService _log;
    private readonly INotificationService _notifications;
    private Invoice _draft = new();

    [ObservableProperty]
    private int invoiceId;

    [ObservableProperty]
    private object? inlineCreator;

    public bool IsInlineCreatorVisible => InlineCreator != null;

    partial void OnInlineCreatorChanged(object? value)
        => OnPropertyChanged(nameof(IsInlineCreatorVisible));

    public bool IsSavePromptVisible => SavePrompt != null;
    public bool IsArchivePromptVisible => ArchivePrompt != null;

    [ObservableProperty]
    private object? savePrompt;

    [ObservableProperty]
    private object? archivePrompt;

    [ObservableProperty]
    private bool isInLineFinalizationPrompt;

    [ObservableProperty]
    private string lastFocusedField = string.Empty;

    partial void OnSavePromptChanged(object? value)
        => OnPropertyChanged(nameof(IsSavePromptVisible));

    partial void OnArchivePromptChanged(object? value)
        => OnPropertyChanged(nameof(IsArchivePromptVisible));

    public InvoiceEditorViewModel(
        IPaymentMethodService paymentMethods,
        ITaxRateService taxRates,
        ISupplierService suppliers,
        IProductService products,
        IUnitService units,
        IInvoiceService invoiceService,
        ILogService logService,
        INotificationService notificationService,
        InvoiceLookupViewModel lookup)
    {
        _paymentMethods = paymentMethods;
        _taxRates = taxRates;
        _suppliers = suppliers;
        _productsService = products;
        _unitsService = units;
        _invoiceService = invoiceService;
        _log = logService;
        _notifications = notificationService;
        Lookup = lookup;
        Lookup.InvoiceSelected += async item =>
        {
            if (Lookup.InlinePrompt is null)
                await LoadInvoice(item.Id, item.Number);
        };
        Items = new ObservableCollection<InvoiceItemRowViewModel>(
            Enumerable.Range(1, 3).Select(i => new InvoiceItemRowViewModel(this)
            {
                IsEditable = i == 1,
                IsFirstRow = i == 1
            }));
    }

    public async Task LoadAsync(IProgress<ProgressReport>? progress = null)
    {
        const int total = 5;
        var step = 0;

        await LoadCollectionAsync(PaymentMethods, _paymentMethods.GetActiveAsync(), Resources.Strings.Load_PaymentMethods, step++, total, progress);
        await LoadCollectionAsync(Suppliers, _suppliers.GetActiveAsync(), Resources.Strings.Load_Suppliers, step++, total, progress);
        await LoadCollectionAsync(TaxRates, _taxRates.GetActiveAsync(DateTime.UtcNow), Resources.Strings.Load_TaxRates, step++, total, progress);
        await LoadCollectionAsync(Products, _productsService.GetActiveAsync(), Resources.Strings.Load_Products, step++, total, progress);
        await LoadCollectionAsync(Units, _unitsService.GetActiveAsync(), Resources.Strings.Load_Units, step++, total, progress);

        progress?.Report(new ProgressReport { GlobalPercent = 100, SubtaskPercent = 100, Message = Resources.Strings.Load_Complete });
    }

    public async Task CheckProductAsync(InvoiceItemRowViewModel row, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;

        var exists = Products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (exists is null)
        {
            InlineCreator = new ProductCreatorViewModel(this, row, _productsService)
            {
                Name = name
            };
        }
        else
        {
            row.Product = exists.Name;
            row.UnitId = exists.UnitId;
            row.UnitName = Units.FirstOrDefault(u => u.Id == exists.UnitId)?.Name ?? string.Empty;
            row.ProductGroup = exists.ProductGroup?.Name ?? string.Empty;
            row.TaxRateId = exists.TaxRateId;
            row.TaxRateName = TaxRates.FirstOrDefault(t => t.Id == exists.TaxRateId)?.Name ?? string.Empty;

            if (SupplierId > 0)
            {
                var usage = await _invoiceService.GetLastUsageDataAsync(SupplierId, exists.Id);
                if (usage != null)
                {
                    row.Quantity = usage.Quantity;
                    row.UnitPrice = usage.UnitPrice;
                    row.TaxRateId = usage.TaxRateId;
                    row.IsAutofilled = true;
                }
                else
                {
                    row.IsAutofilled = false;
                }
            }
        }
    }
private void UpdateSupplierId(string name)
{
    var match = Suppliers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    if (match != null)
        SupplierId = match.Id;
}
    [RelayCommand]
    private void ShowSupplierCreator(string? name)
    {
        InlineCreator = new SupplierCreatorViewModel(this, _suppliers)
        {
            Name = name ?? string.Empty
        };
    }
    [RelayCommand]
    private void ShowProductCreator(InvoiceItemRowViewModel row)
    {
        InlineCreator = new ProductCreatorViewModel(this, row, _productsService)
        {
            Name = row.Product
        };
    }

    [RelayCommand]
    private void ShowTaxRateCreator(string name)
    {
        InlineCreator = new TaxRateCreatorViewModel(this, _taxRates)
        {
            Name = name
        };
    }

    [RelayCommand]
    private void ShowUnitCreator()
    {
        InlineCreator = new UnitCreatorViewModel(this, _unitsService);
    }

    [RelayCommand]
    private void ShowPaymentMethodCreator()
    {
        InlineCreator = new PaymentMethodCreatorViewModel(this, _paymentMethods);
    }

    [RelayCommand]
    internal async Task OpenSelectedInvoiceAsync()
    {
        if (Lookup.SelectedInvoice != null)
            await LoadInvoice(Lookup.SelectedInvoice.Id, Lookup.SelectedInvoice.Number);
    }

    public async Task LoadInvoice(int id, string? number = null)
    {
        if (id == 0)
        {
            _draft = new Invoice();
            InvoiceId = 0;
            IsNew = true;
            SupplierId = 0;
            Supplier = string.Empty;
            InvoiceDate = DateTime.Today;
            Number = number ?? string.Empty;
            PaymentMethodId = Guid.Empty;
            IsGross = false;
            IsArchived = false;
            Items.Clear();
            Items.Add(new InvoiceItemRowViewModel(this) { IsEditable = true, IsFirstRow = true });
            RecalculateTotals();
            FormNavigator.RequestFocus("EntryProduct");
            return;
        }

        var invoice = await _invoiceService.GetAsync(id);
        if (invoice == null)
            return;

        InvoiceId = invoice.Id;

        IsNew = false;

        SupplierId = invoice.SupplierId;
        Supplier = invoice.Supplier?.Name ?? string.Empty;
        InvoiceDate = invoice.Date.ToDateTime(TimeOnly.MinValue);
        Number = invoice.Number;
        PaymentMethodId = invoice.PaymentMethodId;
        IsGross = invoice.IsGross;
        IsArchived = invoice.IsArchived;

        Items.Clear();
        Items.Add(new InvoiceItemRowViewModel(this) { IsEditable = true, IsFirstRow = true });
        foreach (var item in invoice.Items)
        {
            var row = new InvoiceItemRowViewModel(this)
            {
                Product = item.Product?.Name ?? string.Empty,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TaxRateId = item.TaxRateId,
                UnitId = item.Product?.UnitId ?? Guid.Empty,
                UnitName = item.Product?.Unit?.Name ?? string.Empty,
                TaxRateName = item.TaxRate?.Name ?? string.Empty,
                ProductGroup = item.Product?.ProductGroup?.Name ?? string.Empty,
                IsEditable = false
            };
            Items.Add(row);
        }
        RecalculateTotals();
        FormNavigator.RequestFocus("InvoiceList");
    }

    public void EditLineFromSelection(InvoiceItemRowViewModel selected)
    {
        if (!IsEditable)
        {
            NotifyReadOnly();
            return;
        }

        if (Items.IndexOf(selected) <= 0) return;
        var edit = Items[0];
        edit.Product = selected.Product;
        edit.Quantity = selected.Quantity;
        edit.UnitPrice = selected.UnitPrice;
        edit.TaxRateId = selected.TaxRateId;
        edit.UnitId = selected.UnitId;
        edit.UnitName = selected.UnitName;
        edit.TaxRateName = selected.TaxRateName;
        edit.ProductGroup = selected.ProductGroup;
    }

    private bool ValidateLineItem(InvoiceItemRowViewModel line, out string error)
    {
        if (line.Quantity <= 0)
        {
            error = Resources.Strings.InvoiceLine_InvalidQuantity;
            return false;
        }

        if (line.UnitPrice < 0)
        {
            error = Resources.Strings.InvoiceLine_InvalidPrice;
            return false;
        }

        if (line.TaxRateId == Guid.Empty)
        {
            error = Resources.Strings.InvoiceLine_TaxRequired;
            return false;
        }

        error = string.Empty;
        return true;
    }

    private void NotifyReadOnly()
    {
        if (Items.Count > 0)
        {
            var edit = Items[0];
            edit.HasError = true;
            edit.ErrorMessage = Resources.Strings.InvoiceEditor_ReadOnly;
        }
    }

    [RelayCommand]
    private void ShowSavePrompt()
    {
        if (SavePrompt is null)
            SavePrompt = new SaveLinePromptViewModel(this);
    }

    [RelayCommand]
    internal async Task AddLineItemAsync()
    {
        if (!IsEditable)
        {
            NotifyReadOnly();
            return;
        }

        var edit = Items[0];
        if (string.IsNullOrWhiteSpace(edit.Product)) return;

        if (!ValidateLineItem(edit, out var error))
        {
            edit.HasError = true;
            edit.ErrorMessage = error;
            return;
        }

        edit.HasError = false;
        edit.ErrorMessage = string.Empty;

        var product = Products.FirstOrDefault(p => p.Name.Equals(edit.Product, StringComparison.OrdinalIgnoreCase));
        if (product == null) return;

        var item = new InvoiceItem
        {
            ProductId = product.Id,
            TaxRateId = edit.TaxRateId != Guid.Empty ? edit.TaxRateId : product.TaxRateId,
            Quantity = edit.Quantity,
            UnitPrice = edit.UnitPrice
        };

        if (IsNew)
        {
            _draft.Items.Add(item);
        }
        else
        {
            try
            {
                item.InvoiceId = InvoiceId;
                await _invoiceService.AddItemAsync(item);
            }
            catch (Exception ex)
            {
                await _log.LogError("AddLineItemAsync", ex);
                _notifications.ShowError("A sor mentése nem sikerült: " + ex.Message);
                return;
            }
        }

        var row = new InvoiceItemRowViewModel(this)
        {
            Product = product.Name,
            Quantity = edit.Quantity,
            UnitPrice = edit.UnitPrice,
            TaxRateId = item.TaxRateId,
            UnitId = edit.UnitId,
            UnitName = edit.UnitName,
            TaxRateName = TaxRates.FirstOrDefault(t => t.Id == item.TaxRateId)?.Name ?? string.Empty,
            ProductGroup = edit.ProductGroup,
            IsEditable = false
        };

        Items.Insert(1, row);
        RecalculateTotals();
        edit.Product = string.Empty;
        edit.Quantity = 0;
        edit.UnitPrice = 0;
        edit.TaxRateId = Guid.Empty;
        edit.UnitId = Guid.Empty;
        edit.UnitName = string.Empty;
        edit.TaxRateName = string.Empty;
        edit.ProductGroup = string.Empty;
        edit.IsAutofilled = false;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SupplierId <= 0 || PaymentMethodId == Guid.Empty || InvoiceDate is null)
            return;

        var date = DateOnly.FromDateTime(InvoiceDate.Value);

        try
        {
            if (IsNew)
            {
                _draft.Number = Number;
                _draft.SupplierId = SupplierId;
                _draft.PaymentMethodId = PaymentMethodId;
                _draft.Date = date;
                _draft.IsGross = IsGross;
                var ok = await _invoiceService.CreateAsync(_draft);
                if (ok)
                {
                    InvoiceId = _draft.Id;
                    IsNew = false;
                    _draft = new Invoice();
                }
            }
            else
            {
                await _invoiceService.UpdateInvoiceHeaderAsync(InvoiceId, date, SupplierId, PaymentMethodId, IsGross);
            }
        }
        catch (Exception ex)
        {
            await _log.LogError("SaveAsync", ex);
            _notifications.ShowError("A számla mentése nem sikerült: " + ex.Message);
        }
    }

    [RelayCommand]
    private void ShowArchivePrompt()
    {
        if (ArchivePrompt is null)
            ArchivePrompt = new ArchivePromptViewModel(this);
    }

    [RelayCommand]
    internal async Task FinalizeInvoiceAsync()
    {
        await ArchiveAsync();
        IsInLineFinalizationPrompt = false;
    }

    [RelayCommand]
    internal async Task ArchiveAsync()
    {
        if (IsArchived)
            return;

        await _invoiceService.ArchiveAsync(InvoiceId);
        IsArchived = true;
    }

    private async void LookupLoadSelected()
    {
        if (Lookup.SelectedInvoice != null)
            await LoadInvoice(Lookup.SelectedInvoice.Id);
    }

    private void RecalculateTotals()
    {
        decimal net = 0;
        decimal vat = 0;
        decimal gross = 0;
        VatSummaries.Clear();
        var byTax = new Dictionary<Guid, InvoiceTotals>();

        foreach (var row in Items.Skip(1))
        {
            var tax = TaxRates.FirstOrDefault(t => t.Id == row.TaxRateId);
            if (tax is null) continue;

            decimal netUnit = IsGross
                ? row.UnitPrice / (1 + tax.Percentage / 100m)
                : row.UnitPrice;

            decimal netAmount = row.Quantity * netUnit;
            decimal vatAmount = netAmount * (tax.Percentage / 100m);
            decimal grossAmount = netAmount + vatAmount;

            net += netAmount;
            vat += vatAmount;
            gross += grossAmount;

            if (!byTax.TryGetValue(tax.Id, out var totals))
            {
                totals = new InvoiceTotals();
                byTax[tax.Id] = totals;
            }
            totals.Net += netAmount;
            totals.Tax += vatAmount;
            totals.Gross += grossAmount;
        }

        NetTotal = Math.Round(net, 2, MidpointRounding.AwayFromZero);
        VatTotal = Math.Round(vat, 2, MidpointRounding.AwayFromZero);
        GrossTotal = Math.Round(gross, 2, MidpointRounding.AwayFromZero);
        foreach (var kv in byTax)
        {
            var name = TaxRates.FirstOrDefault(t => t.Id == kv.Key)?.Name ?? string.Empty;
            VatSummaries.Add(new VatSummaryRowViewModel
            {
                Rate = name,
                Net = Math.Round(kv.Value.Net, 2, MidpointRounding.AwayFromZero),
                Vat = Math.Round(kv.Value.Tax, 2, MidpointRounding.AwayFromZero),
                Gross = Math.Round(kv.Value.Gross, 2, MidpointRounding.AwayFromZero)
            });
        }
        AmountInWords = NumberToWordsConverter.Convert((long)GrossTotal) + " Ft";
    }
}
