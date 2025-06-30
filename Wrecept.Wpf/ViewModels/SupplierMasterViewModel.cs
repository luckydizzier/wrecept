using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class SupplierMasterViewModel : ObservableObject
{
    public ObservableCollection<Supplier> Suppliers { get; } = new();

    private readonly ISupplierService _service;

    public SupplierMasterViewModel(ISupplierService service)
    {
        _service = service;
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetActiveAsync();
        Suppliers.Clear();
        foreach (var item in items)
            Suppliers.Add(item);
    }
}
