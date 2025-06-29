using System.Windows;
using System.Windows.Input;

namespace Wrecept.Wpf;

public static class NavigationHelper
{
    public static void Handle(KeyEventArgs e)
    {
        var element = e.OriginalSource as UIElement;

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
                e.Handled = true;
                Application.Current.MainWindow?.Focus();
                break;
        }
    }
}
