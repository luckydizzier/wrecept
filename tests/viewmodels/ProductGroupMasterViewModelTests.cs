using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Tests.ViewModels;

public class ProductGroupMasterViewModelTests
{
    private class FakeService : IProductGroupService
    {
        public List<ProductGroup> Groups { get; } = new();
        public ProductGroup? Updated;
        public Task<List<ProductGroup>> GetAllAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Groups);
        public Task<List<ProductGroup>> GetActiveAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Groups);
        public Task<Guid> AddAsync(ProductGroup group, System.Threading.CancellationToken ct = default)
        {
            group.Id = Guid.NewGuid();
            Groups.Add(group);
            return Task.FromResult(group.Id);
        }
        public Task UpdateAsync(ProductGroup group, System.Threading.CancellationToken ct = default)
        {
            Updated = group;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task LoadAsync_FillsCollection()
    {
        var service = new FakeService();
        service.Groups.Add(new ProductGroup { Id = Guid.NewGuid(), Name = "G" });
        var vm = new ProductGroupMasterViewModel(service);

        await vm.LoadAsync();

        Assert.Single(vm.ProductGroups);
    }

    [Fact]
    public async Task DeleteSelectedCommand_ArchivesItem()
    {
        var service = new FakeService();
        var group = new ProductGroup { Id = Guid.NewGuid(), Name = "X" };
        service.Groups.Add(group);
        var vm = new ProductGroupMasterViewModel(service);
        await vm.LoadAsync();

        vm.SelectedItem = vm.ProductGroups[0];
        vm.DeleteSelectedCommand.Execute(null);
        await Task.Delay(10);

        Assert.True(group.IsArchived);
        Assert.Equal(group, service.Updated);
    }
}
