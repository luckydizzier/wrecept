using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;

namespace Wrecept.Tests;

public class PlaceholderViewTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    [StaFact]
    public void Constructor_NoArgs_UsesAppProvider()
    {
        EnsureApp();
        var services = new ServiceCollection();
        services.AddTransient<PlaceholderViewModel>();
        App.Services = services.BuildServiceProvider();

        var view = new PlaceholderView();

        Assert.IsType<PlaceholderViewModel>(view.DataContext);
    }

    [StaFact]
    public void Constructor_WithViewModel_SetsDataContext()
    {
        EnsureApp();
        var vm = new PlaceholderViewModel();

        var view = new PlaceholderView(vm);

        Assert.Same(vm, view.DataContext);
    }
}
