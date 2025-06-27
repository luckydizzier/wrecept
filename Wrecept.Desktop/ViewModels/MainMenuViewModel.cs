using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Desktop.ViewModels;

public partial class MainMenuViewModel : ObservableObject
{
    [ObservableProperty]
    private int selectedIndex;

    public event Action<int>? ItemActivated;

    public IRelayCommand MoveUpCommand { get; }
    public IRelayCommand MoveDownCommand { get; }
    public IRelayCommand EnterCommand { get; }

    public MainMenuViewModel()
    {
        MoveUpCommand = new RelayCommand(() => ChangeSelection(-1));
        MoveDownCommand = new RelayCommand(() => ChangeSelection(1));
        EnterCommand = new RelayCommand(OnEnter);
    }

    private void ChangeSelection(int delta)
    {
        var newIndex = SelectedIndex + delta;
        if (newIndex < 0) newIndex = 0;
        if (newIndex > 3) newIndex = 3;
        SelectedIndex = newIndex;
    }

    private void OnEnter()
    {
        ItemActivated?.Invoke(SelectedIndex);
    }
}
