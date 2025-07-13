namespace InvoiceApp.Core.Services;

public interface INotificationService
{
    void ShowError(string message);
    void ShowInfo(string message);
    bool Confirm(string message);
}
