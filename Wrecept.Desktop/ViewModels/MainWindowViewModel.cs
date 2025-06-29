using System.Windows;
using Wrecept.Desktop;
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
    public IRelayCommand SelectMainMenuCommand { get; }
    public IRelayCommand SelectSubMenuCommand { get; }

    public StageViewModel Stage => _stage;

    public MainWindowViewModel(StageViewModel stage)
    {
        _stage = stage;
        MoveLeftCommand = new RelayCommand(() => { if(!IsSubMenuOpen) ChangeMain(-1); });
        MoveRightCommand = new RelayCommand(() => { if(!IsSubMenuOpen) ChangeMain(1); });
        MoveUpCommand = new RelayCommand(() => { if(IsSubMenuOpen) ChangeSub(-1); });
        MoveDownCommand = new RelayCommand(() => { if(IsSubMenuOpen) ChangeSub(1); });
        EnterCommand = new RelayCommand(OnEnter);
        EscapeCommand = new RelayCommand(OnEscape);
        SelectMainMenuCommand = new RelayCommand<string>(p =>
        {
            if (int.TryParse(p, out var idx))
            {
                SelectedIndex = idx;
                IsSubMenuOpen = true;
                SelectedSubmenuIndex = 0;
            }
        });
        SelectSubMenuCommand = new RelayCommand<string>(p =>
        {
            var parts = p.Split('|');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out var main) &&
                int.TryParse(parts[1], out var sub))
            {
                ActivateMenuItem((MainMenu)main, sub);
            }
        });
    }

    public void ActivateMenuItem(MainMenu main, int subIndex)
    {
        SelectedIndex = (int)main;
        SelectedSubmenuIndex = subIndex;
        IsSubMenuOpen = true;
        ExecuteCurrent();
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
        if (SelectedIndex == 0)
        {
            switch (SelectedSubmenuIndex)
            {
                case 0:
                    _stage.OpenInvoiceEditor();
                    break;
                case 1:
                    // TODO: bejövő számlák aktualizálása
                    break;
            }
        }
        else if (SelectedIndex == 1)
        {
            switch (SelectedSubmenuIndex)
            {
                case 0:
                    _stage.OpenProductView();
                    break;
                case 1:
                    _stage.OpenProductGroupView();
                    break;
                case 2:
                    _stage.OpenSupplierLookupView();
                    break;
                case 3:
                    _stage.OpenTaxRateView();
                    break;
                case 4:
                    _stage.OpenPaymentMethodView();
                    break;
            }
        }
        else if (SelectedIndex == 2)
        {
            MessageBox.Show("A listák funkció még nincs implementálva.", "Listák");
        }
        else if (SelectedIndex == 3)
        {
            MessageBox.Show("A szerviz funkció még nincs implementálva.", "Szerviz");
        }
        else if (SelectedIndex == 4)
        {
            var info = $"Felhasználó: {Environment.UserName}\nVerzió: {BuildInfo.Version}\nCommit: {BuildInfo.CommitHash}\nBuild idő: {BuildInfo.BuildTime:u}";
            MessageBox.Show(info, "Wrecept");
        }
        else if (SelectedIndex == 5)
        {
            if (SelectedSubmenuIndex == 0)
                Application.Current.Shutdown();
        }

        IsSubMenuOpen = false;
    }
}
