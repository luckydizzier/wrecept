using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.ViewModels;

public partial class UnitCreatorViewModel : ObservableObject
{
    private readonly InvoiceEditorViewModel _parent;
    private readonly IUnitService _units;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string code = string.Empty;

    public UnitCreatorViewModel(InvoiceEditorViewModel parent, IUnitService units)
    {
        _parent = parent;
        _units = units;
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        var unit = new Unit { Name = Name, Code = Code };
        var id = await _units.AddAsync(unit);
        unit.Id = id;
        _parent.Units.Add(unit);
        _parent.InlineCreator = null;
    }

    [RelayCommand]
    private void Cancel()
        => _parent.InlineCreator = null;
}
