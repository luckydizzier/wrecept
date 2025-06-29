using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Desktop.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly StageViewModel _stage;
    private readonly int[] _itemCounts = { 2, 5, 4, 4, 1, 1 };

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
        MoveLeftCommand = new RelayCommand(() => { if(!IsSubMenuOpen) ChangeMain(-1); });
        MoveRightCommand = new RelayCommand(() => { if(!IsSubMenuOpen) ChangeMain(1); });
        MoveUpCommand = new RelayCommand(() => { if(IsSubMenuOpen) ChangeSub(-1); });
        MoveDownCommand = new RelayCommand(() => { if(IsSubMenuOpen) ChangeSub(1); });
        EnterCommand = new RelayCommand(OnEnter);
        EscapeCommand = new RelayCommand(OnEscape);
    }

    private void ChangeMain(int delta)
    {
        var count = _itemCounts.Length;
        SelectedIndex = (SelectedIndex + delta + count) % count;
    }

    private void ChangeSub(int delta)
    {
        var count = _itemCounts[SelectedIndex];
        var newIndex = SelectedSubmenuIndex + delta;
        if (newIndex < 0) newIndex = 0;
        if (newIndex >= count) newIndex = count - 1;
        SelectedSubmenuIndex = newIndex;
    }

    private void OnEnter()
    {
        if (!IsSubMenuOpen)
        {
            IsSubMenuOpen = true;
            SelectedSubmenuIndex = 0;
        }
        else
        {
            ExecuteCurrent();
        }
    }

    private void OnEscape()
    {
        if (IsSubMenuOpen)
            IsSubMenuOpen = false;
    }

    private void ExecuteCurrent()
    {
        _stage.HideAll();
        if (SelectedIndex == 1)
        {
            switch (SelectedSubmenuIndex)
            {
                case 1:
                    _stage.ShowProductGroup = true;
                    break;
                case 3:
                    _stage.ShowTaxRate = true;
                    break;
                case 4:
                    _stage.ShowPaymentMethod = true;
                    break;
            }
        }
        IsSubMenuOpen = false;
    }
}
