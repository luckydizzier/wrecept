using System.Windows;

namespace Wrecept.UI.Services;

public class MessageService : IMessageService
{
    public void Show(string message, string caption = "Information")
    {
        MessageBox.Show(message, caption);
    }

    public bool Confirm(string message, string caption = "Confirm")
    {
        var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }
}
