using System.Windows;


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

}
