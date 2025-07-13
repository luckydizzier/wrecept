using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Tests.ViewModels;

public class ProductMasterViewModelTests
{
    private class FakeProductService : IProductService
    {
        public List<Product> Products { get; } = new();
        public Product? Updated;
        public Task<List<Product>> GetAllAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Products);
        public Task<List<Product>> GetActiveAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Products);
        public Task<int> AddAsync(Product product, System.Threading.CancellationToken ct = default)
        {
            product.Id = Products.Count + 1;
            Products.Add(product);
            return Task.FromResult(product.Id);
        }
        public Task UpdateAsync(Product product, System.Threading.CancellationToken ct = default)
        {
            Updated = product;
            return Task.CompletedTask;
        }
    }

    private class FakeTaxRateService : ITaxRateService
    {
        public List<TaxRate> Rates { get; } = new();
        public Task<List<TaxRate>> GetAllAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Rates);
        public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, System.Threading.CancellationToken ct = default)
            => Task.FromResult(Rates);
        public Task<Guid> AddAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default)
            => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default)
            => Task.CompletedTask;
    }

    [Fact]
    public async Task LoadAsync_FillsCollections()
    {
        var productSvc = new FakeProductService();
        var taxSvc = new FakeTaxRateService();
        productSvc.Products.Add(new Product { Id = 1, Name = "Test" });
        taxSvc.Rates.Add(new TaxRate { Id = Guid.NewGuid(), Percentage = 27 });

        var vm = new ProductMasterViewModel(productSvc, taxSvc);

        await vm.LoadAsync();

        Assert.Single(vm.Products);
        Assert.Single(vm.TaxRates);
    }

    [Fact]
    public async Task DeleteSelectedCommand_ArchivesItem()
    {
        var productSvc = new FakeProductService();
        var taxSvc = new FakeTaxRateService();
        var product = new Product { Id = 1, Name = "Del" };
        productSvc.Products.Add(product);
        var vm = new ProductMasterViewModel(productSvc, taxSvc);
        await vm.LoadAsync();

        vm.SelectedItem = vm.Products[0];
        vm.DeleteSelectedCommand.Execute(null);
        await Task.Delay(10);

        Assert.True(product.IsArchived);
        Assert.Equal(product, productSvc.Updated);
    }
}
