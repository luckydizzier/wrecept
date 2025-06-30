using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class UnitMasterViewModel : ObservableObject
{
    public ObservableCollection<Unit> Units { get; } = new();

    private readonly IUnitService _service;

    public UnitMasterViewModel(IUnitService service)
    {
        _service = service;
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();
        Units.Clear();
        foreach (var item in items)
            Units.Add(item);
    }
}
