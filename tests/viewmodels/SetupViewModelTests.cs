using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.Tests.ViewModels;

public class SetupViewModelTests
{
    [Fact]
    public void Constructor_SetsPropertiesAndCommands()
    {
        var vm = new SetupViewModel("db.db", "cfg.json");

        Assert.Equal("db.db", vm.DatabasePath);
        Assert.Equal("cfg.json", vm.ConfigPath);
        Assert.True(vm.OkCommand.CanExecute(null));
        Assert.True(vm.CancelCommand.CanExecute(null));
        Assert.NotNull(vm.BrowseDbCommand);
        Assert.NotNull(vm.BrowseConfigCommand);
    }
}
