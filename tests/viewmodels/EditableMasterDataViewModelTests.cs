using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.MAUI.Services;
using InvoiceApp.Core.Enums;

namespace Wrecept.Tests.ViewModels;

public class EditableMasterDataViewModelTests
{
    private class TestViewModel : EditableMasterDataViewModel<string>
    {
        public int DeleteCalls;
        public TestViewModel(AppStateService s) : base(s) { }
        protected override Task<List<string>> GetItemsAsync() => Task.FromResult(new List<string> { "a", "b" });
        protected override Task DeleteAsync() { DeleteCalls++; return Task.CompletedTask; }
    }

    [Fact]
    public async Task LoadAsync_FillsItemsAndCommands()
    {
        var state = new AppStateService("x");
        var vm = new TestViewModel(state);
        await vm.LoadAsync();
        Assert.Equal(2, vm.Items.Count);
        Assert.False(vm.EditSelectedCommand.CanExecute(null));
        vm.SelectedItem = vm.Items[0];
        Assert.True(vm.EditSelectedCommand.CanExecute(null));
        vm.EditSelectedCommand.Execute(null);
        Assert.True(vm.IsEditing);
        vm.CloseDetailsCommand.Execute(null);
        Assert.False(vm.IsEditing);
        vm.DeleteSelectedCommand.Execute(null);
        Assert.Equal(1, vm.DeleteCalls);
    }
}
