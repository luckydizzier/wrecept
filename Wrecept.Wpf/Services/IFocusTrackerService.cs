using System.Windows;
using System.Windows.Input;

namespace Wrecept.Wpf.Services;

public interface IFocusTrackerService
{
    void Update(string viewKey, IInputElement element);
    IInputElement? GetLast(string viewKey);
}
