using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Repositories;

namespace Wrecept.UI.ViewModels;

public class ContactsViewModel : INotifyPropertyChanged
{
    private readonly IRepository<Supplier> _repo;
    public ObservableCollection<Supplier> Suppliers { get; } = new();
    private Supplier? _selectedSupplier;
    public Supplier? SelectedSupplier
    {
        get => _selectedSupplier;
        set { _selectedSupplier = value; OnPropertyChanged(); }
    }

    public ICommand RefreshCommand { get; }

    public ContactsViewModel(IRepository<Supplier> repo)
    {
        _repo = repo;
        RefreshCommand = new RelayCommand(async _ => await RefreshAsync());
        _ = RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        var items = await _repo.GetAllAsync();
        Suppliers.Clear();
        foreach (var s in items)
            Suppliers.Add(s);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
