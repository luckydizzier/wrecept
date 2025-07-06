using System;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class TaxRateMasterViewModel : EditableMasterDataViewModel<TaxRate>
{
    public ObservableCollection<TaxRate> TaxRates => Items;

    private readonly ITaxRateService _service;

    public TaxRateMasterViewModel(ITaxRateService service, AppStateService state)
        : base(state)
    {
        _service = service;
    }

    protected override Task<List<TaxRate>> GetItemsAsync()
        => _service.GetActiveAsync(DateTime.UtcNow);

    protected override async Task DeleteAsync()
    {
        if (SelectedItem != null)
        {
            SelectedItem.IsArchived = true;
            await _service.UpdateAsync(SelectedItem);
        }
    }

}
