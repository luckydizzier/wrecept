using System;
using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using InvoiceApp.Core.Enums;

namespace InvoiceApp.MAUI.Services;

public class FocusManager
{
    private readonly AppStateService _state;
    private readonly Dictionary<AppInteractionState, Func<VisualElement?>> _registry = new();

    public FocusManager(AppStateService state)
    {
        _state = state;
        _state.InteractionStateChanged += OnStateChanged;
    }

    public void Register(AppInteractionState state, Func<VisualElement?> provider)
        => _registry[state] = provider;

    private void OnStateChanged(AppInteractionState state)
    {
        if (_registry.TryGetValue(state, out var provider))
            RequestFocus(provider());
    }

    public void RequestFocus(VisualElement? element)
    {
        if (element is null)
            return;

        MainThread.BeginInvokeOnMainThread(() => element.Focus());
    }
}
