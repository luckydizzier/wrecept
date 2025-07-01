using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;
using System;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductMasterViewModel : ObservableObject
{
    public ObservableCollection<Product> Products { get; } = new();
    public ObservableCollection<TaxRate> TaxRates { get; } = new();

    private readonly IProductService _service;
    private readonly ITaxRateService _taxRates;

    [ObservableProperty]
    private Product? selectedProduct;

    [ObservableProperty]
    private bool isEditing;

    public IRelayCommand EditSelectedCommand { get; }
    public IRelayCommand DeleteSelectedCommand { get; }
    public IRelayCommand CloseDetailsCommand { get; }

    public ProductMasterViewModel(IProductService service, ITaxRateService taxRates)
    {
        _service = service;
        _taxRates = taxRates;
        EditSelectedCommand = new RelayCommand(() => IsEditing = !IsEditing, () => SelectedProduct != null);
        DeleteSelectedCommand = new RelayCommand(async () =>
        {
            if (SelectedProduct != null)
            {
                SelectedProduct.IsArchived = true;
                await _service.UpdateAsync(SelectedProduct);
                await LoadAsync();
            }
        }, () => SelectedProduct != null);
        CloseDetailsCommand = new RelayCommand(() => IsEditing = false);
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetActiveAsync();
        var rates = await _taxRates.GetActiveAsync(DateTime.UtcNow);
        Products.Clear();
        foreach (var item in items)
            Products.Add(item);
        TaxRates.Clear();
        foreach (var r in rates)
            TaxRates.Add(r);
    }
}
