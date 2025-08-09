namespace Wrecept.UI.Services;

public interface IMessageService
{
    void Show(string message, string caption = "Information");
    bool Confirm(string message, string caption = "Confirm");
}
