using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class SupplierCreatorViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;
    private readonly ISupplierService _suppliers;

    [ObservableProperty]
    private string name = string.Empty;

    public SupplierCreatorViewModel(InvoiceEditorViewModel parent, ISupplierService suppliers)
    {
        _parent = parent;
        _suppliers = suppliers;
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        var supplier = new Supplier { Name = Name };
        var id = await _suppliers.AddAsync(supplier);
        supplier.Id = id;
        _parent.Suppliers.Add(supplier);
        _parent.SupplierId = supplier.Id;
        _parent.InlineCreator = null;
    }

    [RelayCommand]
    private void Cancel()
        => _parent.InlineCreator = null;
}
