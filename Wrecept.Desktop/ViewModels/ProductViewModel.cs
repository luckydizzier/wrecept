using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.Desktop.ViewModels;

public partial class ProductViewModel : ObservableObject
{
    private readonly IProductRepository _repo;

    [ObservableProperty]
    private ObservableCollection<Product> products = new();

    public ProductViewModel(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task LoadAsync(CancellationToken ct = default)
    {
        var items = await _repo.GetAllAsync(ct);
        Products = new ObservableCollection<Product>(items);
    }
}
