using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductMasterViewModel : EditableMasterDataViewModel<Product>
{
    public ObservableCollection<Product> Products => Items;
    public ObservableCollection<TaxRate> TaxRates { get; } = new();

    private readonly IProductService _service;
    private readonly ITaxRateService _taxRates;

    public ProductMasterViewModel(IProductService service, ITaxRateService taxRates)
    {
        _service = service;
        _taxRates = taxRates;
    }

    protected override Task<List<Product>> GetItemsAsync()
        => _service.GetActiveAsync();

    protected override async Task DeleteAsync()
    {
        if (SelectedItem != null)
        {
            SelectedItem.IsArchived = true;
            await _service.UpdateAsync(SelectedItem);
        }
    }

    public override async Task LoadAsync()
    {
        await base.LoadAsync();
        var rates = await _taxRates.GetActiveAsync(DateTime.UtcNow);
        TaxRates.Clear();
        foreach (var r in rates)
            TaxRates.Add(r);
    }
}
