using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Desktop.ViewModels;

public partial class SupplierLookupViewModel : ObservableObject
{
    private readonly ISupplierRepository _repo;

    public ObservableCollection<Supplier> Suppliers { get; } = new();

    public SupplierLookupViewModel(ISupplierRepository repo)
    {
        _repo = repo;
    }

    public async Task LoadAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        Suppliers.Clear();
        foreach (var s in list)
            Suppliers.Add(s);
    }
}
