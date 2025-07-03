using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;

namespace Wrecept.Wpf.Services;

public class FocusManager
{
    public void RequestFocus(IInputElement? element)
    {
        Application.Current.Dispatcher.BeginInvoke(() => element?.Focus(), DispatcherPriority.Background);
    }

    public void RequestFocus(string elementName, Type? viewType = null)
    {
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            if (Application.Current.MainWindow is null)
                return;

            DependencyObject root = Application.Current.MainWindow;
            if (viewType != null)
            {
                var view = FindElement(root, viewType);
                if (view != null)
                    root = view;
            }

            var target = FindElement(root, elementName);
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

    private static DependencyObject? FindElement(DependencyObject parent, Type targetType)
    {
        if (parent.GetType() == targetType)
            return parent;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            var result = FindElement(child, targetType);
            if (result != null)
                return result;
        }
        return null;
    }
}
