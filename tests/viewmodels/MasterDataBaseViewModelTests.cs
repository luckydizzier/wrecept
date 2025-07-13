using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;

namespace InvoiceApp.Tests.ViewModels;

public class MasterDataBaseViewModelTests
{
    private class FakeVm : MasterDataBaseViewModel<string>
    {
        public bool Changed;
        protected override Task<List<string>> GetItemsAsync()
            => Task.FromResult(new List<string> { "A", "B" });
        protected override void SelectedItemChanged(string? value)
            => Changed = true;
    }

    [Fact]
    public async Task LoadAsync_PopulatesItems()
    {
        var vm = new FakeVm();
        await vm.LoadAsync();
        Assert.Equal(2, vm.Items.Count);
        Assert.Contains("A", vm.Items);
        Assert.Contains("B", vm.Items);
    }

    [Fact]
    public void SelectedItem_Set_RaisesCallback()
    {
        var vm = new FakeVm();
        vm.SelectedItem = "X";
        Assert.True(vm.Changed);
    }
}
