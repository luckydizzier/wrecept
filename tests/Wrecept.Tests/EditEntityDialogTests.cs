using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using Wrecept.Wpf.Dialogs;
using Xunit;

namespace Wrecept.Tests;

public class EditEntityDialogTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void Constructor_SetsContentAndDataContext()
    {
        EnsureApp();
        var vm = new object();

        var dialog = new EditEntityDialog<Grid, object>(vm);

        Assert.Same(vm, dialog.ViewModel);
        Assert.IsType<Grid>(dialog.Content);
        var view = (FrameworkElement)dialog.Content!;
        Assert.Same(vm, view.DataContext);
    }

    [StaFact]
    public void Initialize_CentersToMainWindow()
    {
        EnsureApp();
        var main = new Window { Left = 20, Top = 30, Width = 400, Height = 300 };
        Application.Current.MainWindow = main;
        var vm = new object();
        var dialog = new EditEntityDialog<Grid, object>(vm) { Width = 200, Height = 100 };

        dialog.Initialize(new RelayCommand(() => { }), new RelayCommand(() => { }));

        Assert.Same(main, dialog.Owner);
        Assert.Equal(main.Left + (main.Width - dialog.Width) / 2, dialog.Left);
        Assert.Equal(main.Top + (main.Height - dialog.Height) / 2, dialog.Top);
    }
}
