using System.Windows;
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
}
