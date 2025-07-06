using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class TaxRateCreatorViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;
    private readonly ITaxRateService _taxRates;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string code = string.Empty;

    [ObservableProperty]
    private decimal percentage;

    public TaxRateCreatorViewModel(InvoiceEditorViewModel parent, ITaxRateService taxRates)
    {
        _parent = parent;
        _taxRates = taxRates;
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        var rate = new TaxRate
        {
            Name = Name,
            Percentage = Percentage,
            Code = Code,
            EffectiveFrom = DateTime.UtcNow
        };
        var id = await _taxRates.AddAsync(rate);
        rate.Id = id;
        _parent.TaxRates.Add(rate);
        _parent.InlineCreator = null;
    }

    [RelayCommand]
    private void Cancel()
        => _parent.InlineCreator = null;
}
