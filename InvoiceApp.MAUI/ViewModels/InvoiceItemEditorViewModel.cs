using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;
using static InvoiceApp.MAUI.Resources.Strings;
using InvoiceApp.MAUI.Services;

namespace InvoiceApp.MAUI.ViewModels;

public partial class InvoiceItemEditorViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _owner;
    private readonly IInvoiceService _invoiceService;
    private readonly ILogService _log;
    private readonly INotificationService _notifications;

    public ObservableCollection<InvoiceItemRowViewModel> Items => _owner.Items;

    [ObservableProperty]
    private EditableItemViewModel editableItem;

    public InvoiceItemEditorViewModel(
        InvoiceEditorViewModel owner,
        IInvoiceService invoiceService,
        ILogService log,
        INotificationService notifications)
    {
        _owner = owner;
        _invoiceService = invoiceService;
        _log = log;
        _notifications = notifications;
        editableItem = new NewLineItemViewModel(owner) { IsFirstRow = true };
        Items.Add(editableItem);
    }

    partial void OnEditableItemChanged(EditableItemViewModel value)
    {
        if (Items.Count > 0)
            Items[0] = value;
    }

    private bool ValidateLineItem(InvoiceItemRowViewModel line, out string error)
    {
        if (line.Quantity == 0)
        {
            error = Strings.InvoiceLine_InvalidQuantity;
            return false;
        }

        if (line.UnitPrice < 0)
        {
            error = Strings.InvoiceLine_InvalidPrice;
            return false;
        }

        if (line.TaxRateId == Guid.Empty)
        {
            error = Strings.InvoiceLine_TaxRequired;
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
            edit.ErrorMessage = Strings.InvoiceEditor_ReadOnly;
        }
    }

    [RelayCommand]
    internal async Task AddLineItemAsync()
    {
        if (!_owner.IsEditable)
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

        var product = _owner.Products.FirstOrDefault(p => p.Name.Equals(edit.Product, StringComparison.OrdinalIgnoreCase));
        if (product == null) return;

        var item = new InvoiceItem
        {
            ProductId = product.Id,
            TaxRateId = edit.TaxRateId != Guid.Empty ? edit.TaxRateId : product.TaxRateId,
            Quantity = edit.Quantity,
            UnitPrice = edit.UnitPrice,
            Description = edit.Description
        };

        if (_owner.IsNew)
        {
            _owner.Draft.Items.Add(item);
        }
        else
        {
            try
            {
                item.InvoiceId = _owner.InvoiceId;
                var newId = await _invoiceService.AddItemAsync(item);
                item.Id = newId;
            }
            catch (Exception ex)
            {
                await _log.LogError("AddLineItemAsync", ex);
                _notifications.ShowError("A sor mentése nem sikerült: " + ex.Message);
                return;
            }
        }

        var row = new InvoiceItemRowViewModel(_owner)
        {
            Id = item.Id,
            Product = product.Name,
            Quantity = edit.Quantity,
            UnitPrice = edit.UnitPrice,
            TaxRateId = item.TaxRateId,
            UnitId = edit.UnitId,
            UnitName = edit.UnitName,
            TaxRateName = _owner.TaxRates.FirstOrDefault(t => t.Id == item.TaxRateId)?.Name ?? string.Empty,
            ProductGroup = edit.ProductGroup,
            Description = edit.Description,
            IsEditable = false
        };

        Items.Insert(1, row);
        _owner.Totals.Recalculate(Items.Skip(1), _owner.TaxRates, _owner.IsGross);
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

        if (!_owner.IsEditable)
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

        _owner.Totals.Recalculate(Items.Skip(1), _owner.TaxRates, _owner.IsGross);

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
    }

    internal void EditLineFromSelection(InvoiceItemRowViewModel selected)
    {
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
}
