using InvoiceApp.Core.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace InvoiceApp.MAUI.Services;

public class MessageBoxNotificationService : INotificationService
{
    public void ShowError(string message) =>
        MainThread.BeginInvokeOnMainThread(() =>
            Application.Current?.MainPage?.DisplayAlert("Hiba", message, "OK"));

    public void ShowInfo(string message) =>
        MainThread.BeginInvokeOnMainThread(() =>
            Application.Current?.MainPage?.DisplayAlert("Információ", message, "OK"));

    public bool Confirm(string message)
    {
        var tcs = new TaskCompletionSource<bool>();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var result = await Application.Current!.MainPage!
                .DisplayAlert("Megerősítés", message, "Igen", "Nem");
            tcs.SetResult(result);
        });
        return tcs.Task.GetAwaiter().GetResult();
    }
}
