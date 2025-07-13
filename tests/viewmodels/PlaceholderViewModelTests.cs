using System.ComponentModel;
using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.Tests.ViewModels;

public class PlaceholderViewModelTests
{
    [Fact]
    public void Default_Message_Is_Set()
    {
        var vm = new PlaceholderViewModel();
        Assert.Equal("Funkció fejlesztés alatt", vm.Message);
    }

    [Fact]
    public void Setting_Message_Raises_PropertyChanged()
    {
        var vm = new PlaceholderViewModel();
        bool raised = false;
        vm.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(vm.Message)) raised = true; };
        vm.Message = "Hello";
        Assert.True(raised);
        Assert.Equal("Hello", vm.Message);
    }
}
