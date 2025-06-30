using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class TaxRateMasterViewModel : ObservableObject
{
    public ObservableCollection<TaxRate> TaxRates { get; } = new();

    private readonly ITaxRateService _service;

    public TaxRateMasterViewModel(ITaxRateService service)
    {
        _service = service;
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetActiveAsync(DateTime.UtcNow);
        TaxRates.Clear();
        foreach (var item in items)
            TaxRates.Add(item);
    }
}
