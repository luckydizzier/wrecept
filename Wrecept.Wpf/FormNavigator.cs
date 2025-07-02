using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;

namespace Wrecept.Wpf;

public static class FormNavigator
{
    public static void RequestFocus(string elementName)
    {
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            if (Application.Current.MainWindow is null)
                return;

            var target = FindElement(Application.Current.MainWindow, elementName);
            (target as IInputElement)?.Focus();
        }, DispatcherPriority.Background);
    }

    private static DependencyObject? FindElement(DependencyObject parent, string name)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is FrameworkElement fe && fe.Name == name)
                return fe;
            var result = FindElement(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
}
