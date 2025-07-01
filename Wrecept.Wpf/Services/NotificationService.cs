using System.Windows;

namespace Wrecept.Wpf.Services;

public class NotificationService : INotificationService
{
    public void ShowError(string message)
    {
        MessageBox.Show(message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
