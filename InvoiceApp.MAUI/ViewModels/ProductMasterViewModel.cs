using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;
using InvoiceApp.MAUI.Services;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using InvoiceApp.MAUI;

namespace InvoiceApp.MAUI.ViewModels;

public partial class ProductMasterViewModel : EditableMasterDataViewModel<Product>
{
    public ObservableCollection<Product> Products => Items;
    public ObservableCollection<TaxRate> TaxRates { get; } = new();

    private readonly IProductService _service;
    private readonly ITaxRateService _taxRates;

    public new IRelayCommand EditSelectedCommand { get; }

public ProductMasterViewModel(IProductService service, ITaxRateService taxRates, AppStateService state)
    : base(state)
{
    _service = service;
    _taxRates = taxRates;
    EditSelectedCommand = new RelayCommand(EditSelected, () => SelectedItem != null);
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

    private async void EditSelected()
    {
        try
        {
            if (SelectedItem is null)
                return;

            var vm = new ProductEditorViewModel
            {
                Name = SelectedItem.Name,
                Net = SelectedItem.Net,
                Gross = SelectedItem.Gross,
                TaxRateId = SelectedItem.TaxRateId
            };

            vm.OnOk = async m =>
            {
                SelectedItem.Name = m.Name;
                SelectedItem.Net = m.Net;
                SelectedItem.Gross = m.Gross;
                SelectedItem.TaxRateId = m.TaxRateId;
                await _service.UpdateAsync(SelectedItem);
                await LoadAsync();
            };

            DialogService.EditEntity<Views.EditDialogs.ProductEditorView, ProductEditorViewModel>(
                vm, vm.OkCommand, vm.CancelCommand);
        }
        catch (Exception ex)
        {
            var log = App.Provider.GetRequiredService<ILogService>();
            await log.LogError("ProductMasterViewModel.EditSelected", ex);
        }
    }
}
