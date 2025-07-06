using System.Collections.Generic;
using System.Windows.Input;
using Wrecept.Core.Enums;

namespace Wrecept.Wpf.Services;

public interface IKeyboardHandler
{
    bool HandleKey(KeyEventArgs e);
}

public class KeyboardManager
{
    private readonly AppStateService _state;
    private readonly Dictionary<AppInteractionState, IKeyboardHandler> _handlers = new();

    public KeyboardManager(AppStateService state)
    {
        _state = state;
    }

    public void Register(AppInteractionState state, IKeyboardHandler handler)
        => _handlers[state] = handler;

    public bool Process(KeyEventArgs e)
    {
        if (_handlers.TryGetValue(_state.InteractionState, out var handler))
            return handler.HandleKey(e);
        return false;
    }
}
