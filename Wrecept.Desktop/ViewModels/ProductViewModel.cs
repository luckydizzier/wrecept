using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Services;

namespace Wrecept.Desktop.ViewModels;

public partial class ProductViewModel : ObservableObject
{
    private readonly IProductService _service;
    private Product? _editing;
    private bool _isNew;

    [ObservableProperty]
    private ObservableCollection<Product> products = new();

    [ObservableProperty]
    private int selectedIndex;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private decimal net;

    [ObservableProperty]
    private decimal gross;

    public IRelayCommand UpCommand { get; }
    public IRelayCommand DownCommand { get; }
    public IRelayCommand EnterCommand { get; }
    public IRelayCommand EscapeCommand { get; }

    public ProductViewModel(IProductService service)
    {
        _service = service;
        UpCommand = new RelayCommand(() => Change(-1));
        DownCommand = new RelayCommand(() => Change(1));
        EnterCommand = new RelayCommand(async () => await OnEnter());
        EscapeCommand = new RelayCommand(OnEscape);
    }

    public async Task LoadAsync(CancellationToken ct = default)
    {
        var items = await _service.GetAllAsync(ct);
        Products = new ObservableCollection<Product>(items);
        SelectedIndex = Products.Count > 0 ? 0 : -1;
    }

    private void Change(int delta)
    {
        if (IsEditing || Products.Count == 0) return;
        var count = Products.Count;
        SelectedIndex = (SelectedIndex + delta + count) % count;
    }

    private async Task OnEnter()
    {
        if (!IsEditing)
            StartEdit();
        else
            await SaveAsync();
    }

    private void OnEscape()
    {
        if (IsEditing)
            IsEditing = false;
    }

    private void StartEdit()
    {
        if (SelectedIndex < 0 || SelectedIndex >= Products.Count)
        {
            _editing = new Product();
            _isNew = true;
        }
        else
        {
            _editing = Products[SelectedIndex];
            _isNew = false;
        }

        Name = _editing.Name;
        Net = _editing.Net;
        Gross = _editing.Gross;
        IsEditing = true;
    }

    private async Task SaveAsync()
    {
        if (_editing is null) return;

        _editing.Name = Name;
        _editing.Net = Net;
        _editing.Gross = Gross;

        if (_isNew)
        {
            var id = await _service.AddAsync(_editing);
            _editing.Id = id;
            Products.Add(_editing);
            SelectedIndex = Products.Count - 1;
        }
        else
        {
            await _service.UpdateAsync(_editing);
        }

        IsEditing = false;
    }
}
