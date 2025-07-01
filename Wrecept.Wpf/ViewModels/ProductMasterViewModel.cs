using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Wrecept.Wpf.ViewModels;

public partial class ProductMasterViewModel : MasterDataBaseViewModel<Product>
{
    public ObservableCollection<Product> Products => Items;
    public ObservableCollection<TaxRate> TaxRates { get; } = new();

    private readonly IProductService _service;
    private readonly ITaxRateService _taxRates;

    [ObservableProperty]
    private bool isEditing;

    public IRelayCommand EditSelectedCommand { get; }
    public IRelayCommand DeleteSelectedCommand { get; }
    public IRelayCommand CloseDetailsCommand { get; }

    public ProductMasterViewModel(IProductService service, ITaxRateService taxRates)
    {
        _service = service;
        _taxRates = taxRates;
        EditSelectedCommand = new RelayCommand(() => IsEditing = !IsEditing, () => SelectedItem != null);
        DeleteSelectedCommand = new RelayCommand(async () =>
        {
            if (SelectedItem != null)
            {
                SelectedItem.IsArchived = true;
                await _service.UpdateAsync(SelectedItem);
                await LoadAsync();
            }
        }, () => SelectedItem != null);
        CloseDetailsCommand = new RelayCommand(() => IsEditing = false);
    }

    protected override Task<List<Product>> GetItemsAsync()
        => _service.GetActiveAsync();

    public override async Task LoadAsync()
    {
        await base.LoadAsync();
        var rates = await _taxRates.GetActiveAsync(DateTime.UtcNow);
        TaxRates.Clear();
        foreach (var r in rates)
            TaxRates.Add(r);
    }
}
