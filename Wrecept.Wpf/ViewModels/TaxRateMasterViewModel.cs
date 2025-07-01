using System;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

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
