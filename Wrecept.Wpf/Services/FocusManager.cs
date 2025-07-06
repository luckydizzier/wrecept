using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Wrecept.Wpf.Services;

public class FocusManager
{
    public void RequestFocus(IInputElement? element)
    {
        if (element is null)
            return;

        if (Application.Current.Dispatcher.CheckAccess())
            (element as UIElement)?.Focus();
        else
            Application.Current.Dispatcher.BeginInvoke(() => (element as UIElement)?.Focus(), DispatcherPriority.Background);
    }
}

