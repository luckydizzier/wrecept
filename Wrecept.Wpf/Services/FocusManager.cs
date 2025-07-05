using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using Wrecept.Core.Enums;

namespace Wrecept.Wpf.Services;

public class FocusManager
{
    private readonly Dictionary<string, WeakReference<IInputElement>> _map = new();
    private readonly AppStateService _state;

    public FocusManager() : this(new AppStateService()) { }

    public FocusManager(AppStateService state)
    {
        _state = state;
    }

    public void Update(string viewKey, IInputElement element)
    {
        _map[viewKey] = new WeakReference<IInputElement>(element);
    }

    public IInputElement? GetLast(string viewKey)
    {
        if (_map.TryGetValue(viewKey, out var weak) && weak.TryGetTarget(out var element))
            return element;
        return null;
    }

    public void RequestFocus(IInputElement? element)
    {
        if (_state.Current is AppState.Saving or AppState.DialogOpen or AppState.Error or AppState.PromptActive)
            return;

        Application.Current.Dispatcher.BeginInvoke(() => element?.Focus(), DispatcherPriority.Background);
    }

    public void RequestFocus(string elementName, Type? viewType = null)
    {
        if (_state.Current is AppState.Saving or AppState.DialogOpen or AppState.Error or AppState.PromptActive)
            return;

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
