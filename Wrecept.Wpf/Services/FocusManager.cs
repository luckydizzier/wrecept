using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using Wrecept.Core.Enums;

namespace Wrecept.Wpf.Services;

public class FocusManager
{
    private readonly AppStateService _state;
    private readonly Dictionary<AppInteractionState, Func<UIElement?>> _registry = new();

    public FocusManager(AppStateService state)
    {
        _state = state;
        _state.InteractionStateChanged += OnStateChanged;
    }

    public void Register(AppInteractionState state, Func<UIElement?> provider)
        => _registry[state] = provider;

    private void OnStateChanged(AppInteractionState state)
    {
        if (_registry.TryGetValue(state, out var p))
        {
            var element = p();
            RequestFocus(element);
        }
    }

    public void RequestFocus(UIElement? element)
    {
        if (element is null)
            return;

        element.Dispatcher.InvokeAsync(() =>
        {
            if (!element.Focus())
                Keyboard.Focus(element);
        }, DispatcherPriority.ContextIdle);
    }
}
