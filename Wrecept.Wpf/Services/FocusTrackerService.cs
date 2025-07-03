using System.Collections.Generic;
using System.Windows.Input;

namespace Wrecept.Wpf.Services;

public class FocusTrackerService : IFocusTrackerService
{
    private readonly Dictionary<string, WeakReference<IInputElement>> _map = new();

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
}
