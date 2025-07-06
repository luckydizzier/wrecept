using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Wrecept.Wpf.Dialogs;

namespace Wrecept.Wpf.Services;

public static class NavigationService
{
    public static AppStateService? State { get; set; }

    private static async Task<T> WithStateAsync<T>(Func<T> action)
    {
        if (State is { } state)
        {
            T result = default!;
            await state.WithDialogOpen(async () =>
            {
                result = action();
                await Task.CompletedTask;
            });
            return result;
        }

        return action();
    }

    public static bool ShowCenteredDialog(Window dialog)
    {
        return WithStateAsync(() =>
        {
            if (Application.Current.MainWindow is { } owner)
                DialogHelper.CenterToOwner(dialog, owner);
            return dialog.ShowDialog() == true;
        }).GetAwaiter().GetResult();
    }

    public static bool ShowFileDialog(FileDialog dialog)
    {
        return WithStateAsync(() =>
            dialog.ShowDialog(Application.Current.MainWindow) == true)
            .GetAwaiter().GetResult();
    }
}
