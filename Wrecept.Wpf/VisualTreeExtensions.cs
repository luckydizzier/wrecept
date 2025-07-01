using System.Windows;
using System.Windows.Media;

namespace Wrecept.Wpf;

public static class VisualTreeExtensions
{
    public static T? FindAncestor<T>(this DependencyObject obj) where T : DependencyObject
    {
        DependencyObject? parent = VisualTreeHelper.GetParent(obj);
        while (parent != null && parent is not T)
            parent = VisualTreeHelper.GetParent(parent);
        return parent as T;
    }
}
