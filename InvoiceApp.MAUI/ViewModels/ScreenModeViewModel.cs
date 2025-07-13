using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InvoiceApp.Core;
using InvoiceApp.MAUI.Services;

namespace InvoiceApp.MAUI.ViewModels;

public partial class ScreenModeViewModel : ObservableObject
{
    private readonly ScreenModeManager _manager;

    public ObservableCollection<ScreenMode> Modes { get; } = new(Enum.GetValues<ScreenMode>());

    [ObservableProperty]
    private ScreenMode selectedMode;

    public ScreenModeViewModel(ScreenModeManager manager)
    {
        _manager = manager;
        SelectedMode = manager.CurrentMode;
    }

    [RelayCommand]
    private Task Apply(Window window)
        => _manager.ChangeModeAsync(window, SelectedMode);
}
