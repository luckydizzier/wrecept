using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Core;
using Wrecept.Wpf.Services;

namespace Wrecept.Wpf.ViewModels;

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
    private async Task Apply(Window window)
    {
        if (App.Current.MainWindow is MainWindow main)
            await _manager.ChangeModeAsync(main, SelectedMode);
        window.DialogResult = true;
        window.Close();
    }
}
