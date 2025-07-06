using System.Windows;
using Wrecept.Wpf.Dialogs;
using Xunit;

namespace Wrecept.Tests;

public class DialogHelperTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void CenterToOwner_SetsOwnerAndPosition()
    {
        EnsureApp();
        var owner = new Window { Left = 50, Top = 40, Width = 300, Height = 200 };
        var dialog = new Window { Width = 100, Height = 80 };

        DialogHelper.CenterToOwner(dialog, owner);

        Assert.Same(owner, dialog.Owner);
        Assert.Equal(owner.Left + (owner.Width - dialog.Width) / 2, dialog.Left);
        Assert.Equal(owner.Top + (owner.Height - dialog.Height) / 2, dialog.Top);
    }
}
