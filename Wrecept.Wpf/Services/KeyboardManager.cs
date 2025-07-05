using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Wrecept.Core.Enums;

namespace Wrecept.Wpf.Services;

public enum NavigationProfile
{
    Default,
    Disabled
}

public class KeyboardManager
{
    private readonly KeyboardProfile _keys;
    private readonly AppStateService _state;

    public NavigationProfile Profile { get; set; } = NavigationProfile.Default;

    public KeyboardManager(AppStateService state) : this(new KeyboardProfile(), state) { }

    public KeyboardManager(KeyboardProfile keys, AppStateService state)
    {
        _keys = keys;
        _state = state;
    }

    public void Handle(KeyEventArgs e)
    {
        if (_state.Current is AppState.Saving or AppState.DialogOpen)
            return;
        if (Profile == NavigationProfile.Disabled)
            return;

        var element = e.OriginalSource as UIElement;

        if (e.OriginalSource is DependencyObject d)
        {
            if ((e.Key == _keys.Previous || e.Key == _keys.Next) &&
                (d.FindAncestor<ListBox>() is not null ||
                 d.FindAncestor<DataGrid>() is not null ||
                 d.FindAncestor<ComboBox>() is not null ||
                 d.FindAncestor<TreeView>() is not null ||
                 d.FindAncestor<Menu>() is not null ||
                 d.FindAncestor<MenuItem>() is not null))
            {
                return;
            }
        }

        if ((e.Key == _keys.Confirm || e.Key == _keys.Cancel) &&
            e.OriginalSource is TextBox box &&
            box.AcceptsReturn)
        {
            return;
        }

        if (e.Key == _keys.Next)
        {
            e.Handled = true;
            element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        else if (e.Key == _keys.Previous)
        {
            e.Handled = true;
            element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
        }
        else if (e.Key == _keys.Cancel)
        {
            if (Window.GetWindow(element) == Application.Current.MainWindow)
            {
                e.Handled = true;
                Application.Current.MainWindow?.Focus();
            }
        }
        else if (e.Key == _keys.Confirm)
        {
            e.Handled = true;
            element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
