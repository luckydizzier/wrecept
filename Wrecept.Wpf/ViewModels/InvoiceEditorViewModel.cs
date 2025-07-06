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
using System.Windows;
using Wrecept.Wpf.Views.Controls;
using Wrecept.Wpf.Views;
using Controls = Wrecept.Wpf.Views.Controls;
using Wrecept.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

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
    {
        if (!_parent.IsLoading)
            _ = _parent.CheckProductAsync(this, value);
    }

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
    private string description = string.Empty;

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

    internal void SetInitialValues(InvoiceItem item)
    {
        Product = item.Product?.Name ?? string.Empty;
        Quantity = item.Quantity;
        UnitPrice = item.UnitPrice;
        TaxRateId = item.TaxRateId;
        UnitId = item.Product?.UnitId ?? Guid.Empty;
        UnitName = item.Product?.Unit?.Name ?? string.Empty;
        TaxRateName = item.TaxRate?.Name ?? string.Empty;
        ProductGroup = item.Product?.ProductGroup?.Name ?? string.Empty;
        Description = item.Description;
        IsEditable = false;
    }
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

    [ObservableProperty]
    private EditableItemViewModel editableItem = null!;

    public InvoiceLookupViewModel Lookup { get; }

    public ObservableCollection<PaymentMethod> PaymentMethods { get; } = new();
    public ObservableCollection<TaxRate> TaxRates { get; } = new();
    public ObservableCollection<Supplier> Suppliers { get; } = new();
    public ObservableCollection<Product> Products { get; } = new();
    public ObservableCollection<Unit> Units { get; } = new();
    public ObservableCollection<ProductGroup> ProductGroups { get; } = new();

    internal bool IsLoading { get; private set; }

    private static int StepPercent(int step, int total) => step * 100 / total;

    private static void FillCollection<T>(ObservableCollection<T> target, List<T> items, string message, int step, int totalSteps, IProgress<ProgressReport>? progress)
    {
        progress?.Report(new ProgressReport { GlobalPercent = StepPercent(step, totalSteps), SubtaskPercent = 0, Message = message });
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

    partial void OnInvoiceDateChanged(DateTime? value) => RecalculateDueDate();

    [ObservableProperty]
    private DateTime? dueDate;

    [ObservableProperty]
    private string number = string.Empty;

    [ObservableProperty]
    private bool isNew = true;

    [ObservableProperty]
    private Guid paymentMethodId;

    partial void OnPaymentMethodIdChanged(Guid value) => RecalculateDueDate();

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
    private readonly IProductGroupService _groupsService;
    private readonly IInvoiceService _invoiceService;
    private readonly IInvoiceExportService _exporter;
private readonly ILogService _log;
    private readonly INotificationService _notifications;
    private readonly ISessionService _session;
    private readonly AppStateService _state;
private Invoice _draft = new();
private readonly Dictionary<(int, int), LastUsageData> _usageCache = new();

    [ObservableProperty]
    private int invoiceId;

    [ObservableProperty]
    private object? inlineCreator;

    [ObservableProperty]
    private UIElement? inlineCreatorTarget;

    public bool IsInlineCreatorVisible => InlineCreator != null;

    partial void OnInlineCreatorChanged(object? value)
    {
        OnPropertyChanged(nameof(IsInlineCreatorVisible));
        if (value is null)
            InlineCreatorTarget = null;
    }

    public bool IsSavePromptVisible => SavePrompt != null;
    public bool IsArchivePromptVisible => ArchivePrompt != null;
    public bool IsDeletePromptVisible => DeletePrompt != null;

    [ObservableProperty]
    private object? savePrompt;

    [ObservableProperty]
    private object? archivePrompt;

    [ObservableProperty]
    private object? deletePrompt;

    [ObservableProperty]
    private bool isInLineFinalizationPrompt;

    [ObservableProperty]
    private string lastFocusedField = string.Empty;

    partial void OnEditableItemChanged(EditableItemViewModel value)
    {
        if (Items.Count > 0)
            Items[0] = value;
    }

    partial void OnSavePromptChanged(object? value)
        => OnPropertyChanged(nameof(IsSavePromptVisible));

    partial void OnArchivePromptChanged(object? value)
        => OnPropertyChanged(nameof(IsArchivePromptVisible));

    partial void OnDeletePromptChanged(object? value)
        => OnPropertyChanged(nameof(IsDeletePromptVisible));

    public InvoiceEditorViewModel(
        IPaymentMethodService paymentMethods,
        ITaxRateService taxRates,
        ISupplierService suppliers,
        IProductService products,
        IUnitService units,
        IProductGroupService groups,
        IInvoiceService invoiceService,
        IInvoiceExportService exporter,
        ILogService logService,
        INotificationService notificationService,
        ISessionService sessionService,
        AppStateService state,
        InvoiceLookupViewModel lookup)
    {
        _paymentMethods = paymentMethods;
        _taxRates = taxRates;
        _suppliers = suppliers;
        _productsService = products;
        _unitsService = units;
        _groupsService = groups;
        _invoiceService = invoiceService;
        _exporter = exporter;
        _log = logService;
        _notifications = notificationService;
        _session = sessionService;
        _state = state;
        Lookup = lookup;
        Lookup.InvoiceSelected += async item =>
        {
            if (Lookup.InlinePrompt is null)
                await LoadInvoice(item.Id, item.Number);
        };
        Lookup.InvoiceCreated += async number =>
        {
            await LoadInvoice(0, number);
        };
        Items = new ObservableCollection<InvoiceItemRowViewModel>();
        EditableItem = new NewLineItemViewModel(this) { IsFirstRow = true };
        Items.Add(EditableItem);
    }

    public async Task LoadAsync(IProgress<ProgressReport>? progress = null)
    {
        const int total = 6;
        var step = 0;

        IsLoading = true;

        var paymentTask = _paymentMethods.GetActiveAsync();
        var supplierTask = _suppliers.GetActiveAsync();
        var taxTask = _taxRates.GetActiveAsync(DateTime.UtcNow);
        var productTask = _productsService.GetActiveAsync();
        var unitTask = _unitsService.GetActiveAsync();
        var groupTask = _groupsService.GetActiveAsync();

        await Task.WhenAll(paymentTask, supplierTask, taxTask, productTask, unitTask, groupTask);

        FillCollection(PaymentMethods, paymentTask.Result, Resources.Strings.Load_PaymentMethods, step++, total, progress);
        FillCollection(Suppliers, supplierTask.Result, Resources.Strings.Load_Suppliers, step++, total, progress);
        FillCollection(TaxRates, taxTask.Result, Resources.Strings.Load_TaxRates, step++, total, progress);
        FillCollection(Products, productTask.Result, Resources.Strings.Load_Products, step++, total, progress);
        FillCollection(Units, unitTask.Result, Resources.Strings.Load_Units, step++, total, progress);
        FillCollection(ProductGroups, groupTask.Result, Resources.Strings.Load_ProductGroups, step++, total, progress);

        progress?.Report(new ProgressReport { GlobalPercent = 100, SubtaskPercent = 100, Message = Resources.Strings.Load_Complete });
        IsLoading = false;
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
                var usage = await GetUsageDataAsync(SupplierId, exists.Id);
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

    private async Task<LastUsageData?> GetUsageDataAsync(int supplierId, int productId)
    {
        if (_usageCache.TryGetValue((supplierId, productId), out var cached))
            return cached;

        var data = await _invoiceService.GetLastUsageDataAsync(supplierId, productId);
        if (data != null)
            _usageCache[(supplierId, productId)] = data;
        return data;
    }
    [RelayCommand]
    private void ShowSupplierCreator(object? parameter)
    {
        if (parameter is UIElement target)
        {
            InlineCreatorTarget = target;
            var text = (target as Views.Controls.SmartLookup)?.Text ?? string.Empty;
            InlineCreator = new SupplierCreatorViewModel(this, _suppliers)
            {
                Name = text
            };
        }
    }
    [RelayCommand]
    private void ShowProductCreator(object? parameter)
    {
        if (parameter is UIElement target && target is FrameworkElement fe && fe.DataContext is InvoiceItemRowViewModel row)
        {
            InlineCreatorTarget = target;
            var name = (target as Views.Controls.SmartLookup)?.Text ?? row.Product;
            InlineCreator = new ProductCreatorViewModel(this, row, _productsService)
            {
                Name = name
            };
        }
    }

    [RelayCommand]
    private void ShowTaxRateCreator(object? parameter)
    {
        if (parameter is UIElement target)
        {
            InlineCreatorTarget = target;
            var name = (target as Controls.EditLookup)?.Box.Text ?? (target as Controls.SmartLookup)?.Text ?? string.Empty;
            InlineCreator = new TaxRateCreatorViewModel(this, _taxRates)
            {
                Name = name
            };
        }
    }

    [RelayCommand]
    private void ShowUnitCreator(object? parameter)
    {
        if (parameter is UIElement target)
        {
            InlineCreatorTarget = target;
            InlineCreator = new UnitCreatorViewModel(this, _unitsService);
        }
    }

    [RelayCommand]
    private void ShowPaymentMethodCreator(object? parameter)
    {
        if (parameter is UIElement target)
        {
            InlineCreatorTarget = target;
            InlineCreator = new PaymentMethodCreatorViewModel(this, _paymentMethods);
        }
    }

    [RelayCommand]
    internal async Task OpenSelectedInvoiceAsync()
    {
        if (Lookup.SelectedInvoice != null)
            await LoadInvoice(Lookup.SelectedInvoice.Id, Lookup.SelectedInvoice.Number);
    }

    public async Task LoadInvoice(int id, string? number = null)
    {
        IsLoading = true;
        if (id == 0)
        {
            _draft = new Invoice();
            InvoiceId = 0;
            IsNew = true;
            SupplierId = 0;
            Supplier = string.Empty;
            InvoiceDate = DateTime.Today;
            DueDate = null;
            Number = number ?? string.Empty;
            PaymentMethodId = Guid.Empty;
            IsGross = false;
            IsArchived = false;
            Items.Clear();
            EditableItem = new NewLineItemViewModel(this) { IsFirstRow = true };
            Items.Add(EditableItem);
            RecalculateTotals();
            await Task.Yield();
            IsLoading = false;
            await _session.SaveLastInvoiceIdAsync(null);
            _state.CurrentInvoiceId = null;
            await _state.SaveAsync();
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
        DueDate = invoice.DueDate.ToDateTime(TimeOnly.MinValue);
        IsGross = invoice.IsGross;
        IsArchived = invoice.IsArchived;

        Items.Clear();
        EditableItem = new NewLineItemViewModel(this) { IsFirstRow = true };
        Items.Add(EditableItem);
        foreach (var item in invoice.Items)
        {
            var row = new InvoiceItemRowViewModel(this);
            row.SetInitialValues(item);
            Items.Add(row);
        }
        RecalculateTotals();
        await Task.Yield();
        IsLoading = false;
        await _session.SaveLastInvoiceIdAsync(InvoiceId);
        _state.CurrentInvoiceId = InvoiceId;
        await _state.SaveAsync();
    }

    [RelayCommand]
    private async Task SavePdfAsync()
    {
        var dlg = new SaveFileDialog
        {
            Filter = "PDF (*.pdf)|*.pdf",
            FileName = $"{Number}.pdf"
        };
        if (NavigationService.ShowFileDialog(dlg))
        {
            var invoice = await _invoiceService.GetAsync(InvoiceId);
            if (invoice != null)
                await _exporter.SavePdfAsync(invoice, dlg.FileName);
        }
    }

    [RelayCommand]
    private async Task PrintAsync()
    {
        var invoice = await _invoiceService.GetAsync(InvoiceId);
        if (invoice != null)
            await _exporter.PrintAsync(invoice);
    }

    public void EditLineFromSelection(InvoiceItemRowViewModel selected)
    {
        if (!IsEditable)
        {
            NotifyReadOnly();
            return;
        }

        if (Items.IndexOf(selected) <= 0) return;
        var edit = EditableItem;
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
        if (line.Quantity == 0)
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
            var edit = EditableItem;
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

        var edit = EditableItem;
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
            UnitPrice = edit.UnitPrice,
            Description = edit.Description
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
            Description = edit.Description,
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
        edit.Description = string.Empty;
        edit.IsAutofilled = false;
        edit.IsEditingExisting = false;
        edit.TargetRow = null;
    }

    [RelayCommand]
    internal void SaveEditedItem()
    {
        if (EditableItem is not ExistingLineItemEditViewModel edit || edit.TargetRow is null)
            return;

        if (!IsEditable)
        {
            NotifyReadOnly();
            return;
        }

        if (!ValidateLineItem(edit, out var error))
        {
            edit.HasError = true;
            edit.ErrorMessage = error;
            return;
        }

        var target = edit.TargetRow;
        target.Product = edit.Product;
        target.Quantity = edit.Quantity;
        target.UnitPrice = edit.UnitPrice;
        target.TaxRateId = edit.TaxRateId;
        target.UnitId = edit.UnitId;
        target.UnitName = edit.UnitName;
        target.TaxRateName = edit.TaxRateName;
        target.ProductGroup = edit.ProductGroup;
        target.Description = edit.Description;

        edit.HasError = false;
        edit.ErrorMessage = string.Empty;

        RecalculateTotals();

        edit.Product = string.Empty;
        edit.Quantity = 0;
        edit.UnitPrice = 0;
        edit.TaxRateId = Guid.Empty;
        edit.UnitId = Guid.Empty;
        edit.UnitName = string.Empty;
        edit.TaxRateName = string.Empty;
        edit.ProductGroup = string.Empty;
        edit.Description = string.Empty;
        edit.IsEditingExisting = false;
        edit.TargetRow = null;
        // focus removed
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SupplierId <= 0 || PaymentMethodId == Guid.Empty || InvoiceDate is null)
            return;

        var date = DateOnly.FromDateTime(InvoiceDate.Value);
        var dueDate = DueDate != null
            ? DateOnly.FromDateTime(DueDate.Value)
            : date.AddDays(PaymentMethods.FirstOrDefault(m => m.Id == PaymentMethodId)?.DueInDays ?? 0);

        try
        {
            if (IsNew)
            {
                _draft.Number = Number;
                _draft.SupplierId = SupplierId;
                _draft.PaymentMethodId = PaymentMethodId;
                _draft.Date = date;
                _draft.IsGross = IsGross;
                _draft.DueDate = dueDate;
                var ok = await _invoiceService.CreateAsync(_draft);
                if (ok)
                {
                    var newId = _draft.Id;
                    InvoiceId = newId;
                    IsNew = false;
                    _draft = new Invoice();
                    await Lookup.LoadAsync();
                    Lookup.SelectedInvoice = Lookup.Invoices.FirstOrDefault(i => i.Id == newId);
                }
            }
            else
            {
                await _invoiceService.UpdateInvoiceHeaderAsync(InvoiceId, date, dueDate, SupplierId, PaymentMethodId, IsGross);
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

    internal void RequestDeleteItem(InvoiceItemRowViewModel row)
    {
        if (!IsEditable || Items.IndexOf(row) <= 0)
            return;
        if (DeletePrompt is null)
            DeletePrompt = new DeleteItemPromptViewModel(this, row);
    }

    internal void DeleteItemConfirmed(InvoiceItemRowViewModel row)
    {
        var index = Items.IndexOf(row);
        if (index <= 0)
            return;
        Items.RemoveAt(index);
        if (IsNew && index - 1 >= 0 && index - 1 < _draft.Items.Count)
        {
            var domainItem = _draft.Items.ElementAt(index - 1);
            _draft.Items.Remove(domainItem);
        }
        RecalculateTotals();
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
        await _session.SaveLastInvoiceIdAsync(null);
        _state.CurrentInvoiceId = null;
        await _state.SaveAsync();
        await Lookup.LoadAsync();
        Lookup.SelectedInvoice = null;
    }

    private async void LookupLoadSelected()
    {
        try
        {
            if (Lookup.SelectedInvoice != null)
                await LoadInvoice(Lookup.SelectedInvoice.Id);
        }
        catch (Exception ex)
        {
            await _log.LogError("LookupLoadSelected", ex);
        }
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

    private void RecalculateDueDate()
    {
        if (InvoiceDate != null && PaymentMethodId != Guid.Empty)
        {
            var method = PaymentMethods.FirstOrDefault(m => m.Id == PaymentMethodId);
            if (method != null)
                DueDate = InvoiceDate.Value.AddDays(method.DueInDays);
        }
        else
        {
            DueDate = null;
        }
    }
}
