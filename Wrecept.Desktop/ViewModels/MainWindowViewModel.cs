using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Desktop.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private int selectedTab;

    public IRelayCommand MoveLeftCommand { get; }
    public IRelayCommand MoveRightCommand { get; }
    public IRelayCommand EnterCommand { get; }

    public MainWindowViewModel()
    {
        MoveLeftCommand = new RelayCommand(() => ChangeTab(-1));
        MoveRightCommand = new RelayCommand(() => ChangeTab(1));
        EnterCommand = new RelayCommand(() => { });
    }

    private void ChangeTab(int delta)
    {
        var newIndex = SelectedTab + delta;
        if (newIndex < 0) newIndex = 0;
        if (newIndex > 5) newIndex = 5;
        SelectedTab = newIndex;
    }
}
