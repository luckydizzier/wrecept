using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Tests.ViewModels;

public class SupplierMasterViewModelTests
{
    private class FakeSupplierService : ISupplierService
    {
        public List<Supplier> Suppliers { get; } = new();
        public Supplier? Updated;
        public Task<List<Supplier>> GetAllAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Suppliers);
        public Task<List<Supplier>> GetActiveAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Suppliers);
        public Task<int> AddAsync(Supplier supplier, System.Threading.CancellationToken ct = default)
        {
            supplier.Id = Suppliers.Count + 1;
            Suppliers.Add(supplier);
            return Task.FromResult(supplier.Id);
        }
        public Task UpdateAsync(Supplier supplier, System.Threading.CancellationToken ct = default)
        {
            Updated = supplier;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task LoadAsync_FillsCollection()
    {
        var service = new FakeSupplierService();
        service.Suppliers.Add(new Supplier { Id = 1, Name = "Test" });
        var vm = new SupplierMasterViewModel(service);

        await vm.LoadAsync();

        Assert.Single(vm.Suppliers);
    }

    [Fact]
    public async Task DeleteSelectedCommand_ArchivesItem()
    {
        var service = new FakeSupplierService();
        var supplier = new Supplier { Id = 1, Name = "X" };
        service.Suppliers.Add(supplier);
        var vm = new SupplierMasterViewModel(service);
        await vm.LoadAsync();

        vm.SelectedItem = vm.Suppliers[0];
        vm.DeleteSelectedCommand.Execute(null);
        await Task.Delay(10);

        Assert.True(supplier.IsArchived);
        Assert.Equal(supplier, service.Updated);
    }
}
