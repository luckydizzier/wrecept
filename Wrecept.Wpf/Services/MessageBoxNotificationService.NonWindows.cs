#if !WINDOWS
using Wrecept.Core.Services;

namespace Wrecept.Wpf.Services;

public class MessageBoxNotificationService : INotificationService
{
    public void ShowError(string message) { }
    public void ShowInfo(string message) { }
    public bool Confirm(string message) => true;
}
#endif
