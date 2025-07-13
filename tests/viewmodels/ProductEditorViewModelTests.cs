using System;
using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace Wrecept.Tests.ViewModels;

public class ProductEditorViewModelTests
{
    [Fact]
    public void Commands_InvokeDelegates()
    {
        var vm = new ProductEditorViewModel();
        bool ok = false;
        bool cancel = false;
        vm.OnOk = _ => ok = true;
        vm.OnCancel = () => cancel = true;

        vm.OkCommand.Execute(null);
        vm.CancelCommand.Execute(null);

        Assert.True(ok);
        Assert.True(cancel);
    }
}
