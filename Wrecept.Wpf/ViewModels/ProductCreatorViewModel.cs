using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.ObjectModel;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductCreatorViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;
    private readonly InvoiceItemRowViewModel _row;
    private readonly IProductService _products;

    public ObservableCollection<Unit> Units => _parent.Units;
    public ObservableCollection<TaxRate> TaxRates => _parent.TaxRates;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private decimal net;

    [ObservableProperty]
    private decimal gross;

    [ObservableProperty]
    private Guid unitId;

    [ObservableProperty]
    private Guid taxRateId;

    public ProductCreatorViewModel(InvoiceEditorViewModel parent, InvoiceItemRowViewModel row, IProductService products)
    {
        _parent = parent;
        _row = row;
        _products = products;
        name = row.Product;
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        var product = new Product
        {
            Name = Name,
            Net = Net,
            Gross = Gross,
            UnitId = UnitId,
            TaxRateId = TaxRateId
        };

        var id = await _products.AddAsync(product);
        product.Id = id;
        _parent.Products.Add(product);
        _row.Product = product.Name;
        _parent.InlineCreator = null;
        if (_parent.IsEditable)
            _parent.AddLineItemCommand.Execute(null);
    }

    [RelayCommand]
    private void Cancel()
        => _parent.InlineCreator = null;
}
