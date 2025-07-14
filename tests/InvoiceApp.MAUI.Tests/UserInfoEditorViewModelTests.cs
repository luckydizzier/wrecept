using InvoiceApp.MAUI.ViewModels;
using Xunit;

namespace InvoiceApp.Tests.ViewModels;

public class UserInfoEditorViewModelTests
{
    [Fact]
    public void OkCommand_CanExecute_False_WhenFieldsMissing()
    {
        var vm = new UserInfoEditorViewModel();
        Assert.False(vm.OkCommand.CanExecute(null));
    }

    [Fact]
    public void OkCommand_CanExecute_True_WhenAllFieldsFilled()
    {
        var vm = new UserInfoEditorViewModel
        {
            CompanyName = "ACME",
            Address = "Addr",
            Phone = "1",
            Email = "a@b.c",
            TaxNumber = "123",
            BankAccount = "111"
        };
        Assert.True(vm.OkCommand.CanExecute(null));
    }
}
