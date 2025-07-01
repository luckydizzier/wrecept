using System.Windows;
using Wrecept.Core.Services;

namespace Wrecept.Wpf.Services;

public class MessageBoxNotificationService : INotificationService
{
    public void ShowError(string message) =>
        MessageBox.Show(message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);

    public void ShowInfo(string message) =>
        MessageBox.Show(message, "Információ", MessageBoxButton.OK, MessageBoxImage.Information);

    public bool Confirm(string message) =>
        MessageBox.Show(message, "Megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
}
