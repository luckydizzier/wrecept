using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Tests.ViewModels;

public class SupplierCreatorViewModelTests
{
    private static T CreateUninitialized<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));

    private class FakeService : ISupplierService
    {
        public Task<List<Supplier>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<List<Supplier>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<int> AddAsync(Supplier supplier, System.Threading.CancellationToken ct = default)
        {
            supplier.Id = 1;
            return Task.FromResult(supplier.Id);
        }
        public Task UpdateAsync(Supplier supplier, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    [Fact]
    public void Defaults_AreEmptyAndCommandsAvailable()
    {
        var parent = CreateUninitialized<InvoiceEditorViewModel>();
        var vm = new SupplierCreatorViewModel(parent, new FakeService());

        Assert.Equal(string.Empty, vm.Name);
        Assert.Equal(string.Empty, vm.TaxId);
        Assert.True(vm.ConfirmAsyncCommand.CanExecute(null));
        Assert.True(vm.CancelCommand.CanExecute(null));
    }
}
