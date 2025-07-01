using System;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class TaxRateMasterViewModel : MasterDataBaseViewModel<TaxRate>
{
    public ObservableCollection<TaxRate> TaxRates => Items;

    private readonly ITaxRateService _service;

    public TaxRateMasterViewModel(ITaxRateService service)
    {
        _service = service;
    }

    protected override Task<List<TaxRate>> GetItemsAsync()
        => _service.GetActiveAsync(DateTime.UtcNow);
}
