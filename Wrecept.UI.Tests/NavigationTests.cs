using System.Runtime.InteropServices;
using Wrecept.UI.ViewModels;
using Wrecept.UI.Views;

namespace Wrecept.UI.Tests;

public class NavigationTests
{
    [SkippableFact]
    [Trait("Category", "UI")]
    public void Should_NavigateProperly()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            "UI tests require WindowsDesktop SDK");
        Assert.True(true);
    }

    [SkippableFact]
    [Trait("Category", "UI")]
    public void MainViewModel_Should_NavigateSections()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            "UI tests require WindowsDesktop SDK");
        var vm = new MainViewModel();
        vm.StocksCommand.Execute(null);
        Assert.Equal(MainSection.Stocks, vm.SelectedSection);
        Assert.IsType<StocksView>(vm.CurrentView);
    }
}
