using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Wpf.Dialogs;

public static class DialogHelper
{
    public static void CenterToOwner(Window dialog, Window owner)
    {
        dialog.WindowStartupLocation = WindowStartupLocation.Manual;
        dialog.Owner = owner;
        dialog.Left = owner.Left + (owner.Width - dialog.Width) / 2;
        dialog.Top = owner.Top + (owner.Height - dialog.Height) / 2;
    }

    public static void MapKeys(Window dialog, IRelayCommand ok, IRelayCommand cancel)
    {
        dialog.PreviewKeyDown += (_, e) =>
        {
            if (e.Key == Key.Enter)
            {
                if (e.OriginalSource is TextBox box && box.AcceptsReturn)
                    return;

                if (ok.CanExecute(null))
                {
                    ok.Execute(null);
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (cancel.CanExecute(null))
                {
                    cancel.Execute(null);
                    e.Handled = true;
                }
            }
        };
    }
}
