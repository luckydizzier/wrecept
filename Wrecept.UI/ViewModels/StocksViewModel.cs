using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.UI.ViewModels;

public class StocksViewModel : INotifyPropertyChanged
{
    private readonly IRepository<Product> _repo;
    public ObservableCollection<Product> Products { get; } = new();
    private Product? _selectedProduct;
    public Product? SelectedProduct
    {
        get => _selectedProduct;
        set { _selectedProduct = value; OnPropertyChanged(); }
    }

    public ICommand RefreshCommand { get; }

    public StocksViewModel(IRepository<Product> repo)
    {
        _repo = repo;
        RefreshCommand = new RelayCommand(async _ => await RefreshAsync());
        _ = RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        var items = await _repo.GetAllAsync();
        Products.Clear();
        foreach (var p in items)
            Products.Add(p);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
