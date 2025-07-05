using System.Windows;
using Microsoft.Win32;
using Wrecept.Wpf.Dialogs;

namespace Wrecept.Wpf.Services;

public static class NavigationService
{
    public static bool ShowCenteredDialog(Window dialog)
    {
        if (Application.Current.MainWindow is { } owner)
            DialogHelper.CenterToOwner(dialog, owner);
        return dialog.ShowDialog() == true;
    }

    public static bool ShowFileDialog(FileDialog dialog)
    {
        return dialog.ShowDialog(Application.Current.MainWindow) == true;
    }
}
