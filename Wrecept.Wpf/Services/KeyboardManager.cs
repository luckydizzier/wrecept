using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Wrecept.Wpf.Services;

public enum NavigationProfile
{
    Default,
    Disabled
}

public class KeyboardManager
{
    public NavigationProfile Profile { get; set; } = NavigationProfile.Default;

    public void Handle(KeyEventArgs e)
    {
        if (Profile == NavigationProfile.Disabled)
            return;

        var element = e.OriginalSource as UIElement;

        if (e.OriginalSource is DependencyObject d)
        {
            if ((e.Key is Key.Up or Key.Down) &&
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

        if ((e.Key is Key.Enter or Key.Escape) &&
            e.OriginalSource is TextBox box &&
            box.AcceptsReturn)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Down:
            case Key.Enter:
                e.Handled = true;
                element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                break;
            case Key.Up:
                e.Handled = true;
                element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                break;
            case Key.Escape:
                if (Window.GetWindow(element) == Application.Current.MainWindow)
                {
                    e.Handled = true;
                    Application.Current.MainWindow?.Focus();
                }
                break;
        }
    }
}
