using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Tests.ViewModels;

public class ProductCreatorViewModelTests
{
    private static T CreateUninitialized<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));

    private class FakeService : IProductService
    {
        public Task<List<Product>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Product>());
        public Task<List<Product>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Product>());
        public Task<int> AddAsync(Product product, System.Threading.CancellationToken ct = default)
        {
            product.Id = 1;
            return Task.FromResult(product.Id);
        }
        public Task UpdateAsync(Product product, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    [Fact]
    public void Defaults_AreEmptyAndCommandsAvailable()
    {
        var parent = CreateUninitialized<InvoiceEditorViewModel>();
        var row = new InvoiceItemRowViewModel(parent);
        var vm = new ProductCreatorViewModel(parent, row, new FakeService());

        Assert.True(vm.IsInlineOpen);
        Assert.Equal(string.Empty, vm.Name);
        Assert.Equal(0, vm.Net);
        Assert.Equal(0, vm.Gross);
        Assert.True(vm.ConfirmAsyncCommand.CanExecute(null));
        Assert.True(vm.CancelCommand.CanExecute(null));
    }
}
