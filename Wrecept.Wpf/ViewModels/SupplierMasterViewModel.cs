using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class SupplierMasterViewModel : EditableMasterDataViewModel<Supplier>
{
    private readonly ISupplierService _service;

    public SupplierMasterViewModel(ISupplierService service)
    {
        _service = service;
    }

    public ObservableCollection<Supplier> Suppliers => Items;

    protected override Task<List<Supplier>> GetItemsAsync()
        => _service.GetActiveAsync();

    protected override async Task DeleteAsync()
    {
        if (SelectedItem != null)
        {
            SelectedItem.IsArchived = true;
            await _service.UpdateAsync(SelectedItem);
        }
    }
}
