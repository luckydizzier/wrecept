using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Desktop.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly StageViewModel _stage;

    private int SelectedIndex
    {
        get => _stage.SelectedIndex;
        set => _stage.SelectedIndex = value;
    }

    private int SelectedSubmenuIndex
    {
        get => _stage.SelectedSubmenuIndex;
        set => _stage.SelectedSubmenuIndex = value;
    }

    private bool IsSubMenuOpen
    {
        get => _stage.IsSubMenuOpen;
        set => _stage.IsSubMenuOpen = value;
    }

    public IRelayCommand MoveLeftCommand { get; }
    public IRelayCommand MoveRightCommand { get; }
    public IRelayCommand MoveUpCommand { get; }
    public IRelayCommand MoveDownCommand { get; }
    public IRelayCommand EnterCommand { get; }
    public IRelayCommand EscapeCommand { get; }

    public MainWindowViewModel(StageViewModel stage)
    {
        _stage = stage;
        MoveLeftCommand = new RelayCommand(() => { if(IsSubMenuOpen) IsSubMenuOpen = false; });
        MoveRightCommand = new RelayCommand(() => { if(!IsSubMenuOpen) { IsSubMenuOpen = true; SelectedSubmenuIndex = 0; } });
        MoveUpCommand = new RelayCommand(() =>
        {
            if (IsSubMenuOpen)
                SelectNextSubmenu(-1);
            else
                ChangeMain(-1);
        });
        MoveDownCommand = new RelayCommand(() =>
        {
            if (IsSubMenuOpen)
                SelectNextSubmenu(1);
            else
                ChangeMain(1);
        });
        EnterCommand = new RelayCommand(ExecuteSubmenuItem);
        EscapeCommand = new RelayCommand(ReturnToMainTabs);
    }

    private void ChangeMain(int delta)
    {
        const int mainCount = 6;
        SelectedIndex = (SelectedIndex + delta + mainCount) % mainCount;
    }

    private void ChangeSub(int delta)
    {
        var count = _stage.CurrentSubmenuItems.Count;
        var newIndex = SelectedSubmenuIndex + delta;
        if (newIndex < 0) newIndex = 0;
        if (newIndex >= count) newIndex = count - 1;
        SelectedSubmenuIndex = newIndex;
    }

    public void SelectNextSubmenu(int delta)
    {
        var count = _stage.CurrentSubmenuItems.Count;
        if (count == 0) return;
        var newIndex = SelectedSubmenuIndex + delta;
        if (newIndex < 0) newIndex = 0;
        if (newIndex >= count) newIndex = count - 1;
        SelectedSubmenuIndex = newIndex;
    }

    public void ExecuteSubmenuItem()
    {
        if (!IsSubMenuOpen)
        {
            IsSubMenuOpen = true;
            SelectedSubmenuIndex = 0;
        }
        else
        {
            _stage.ExecuteCurrentSubmenu();
            IsSubMenuOpen = false;
        }
    }

    public void ReturnToMainTabs()
    {
        if (IsSubMenuOpen)
            IsSubMenuOpen = false;
    }
}
