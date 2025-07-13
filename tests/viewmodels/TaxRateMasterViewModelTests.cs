using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Tests.ViewModels;

public class TaxRateMasterViewModelTests
{
    private class FakeService : ITaxRateService
    {
        public List<TaxRate> Rates { get; } = new();
        public TaxRate? Updated;
        public Task<List<TaxRate>> GetAllAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Rates);
        public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, System.Threading.CancellationToken ct = default)
            => Task.FromResult(Rates);
        public Task<Guid> AddAsync(TaxRate rate, System.Threading.CancellationToken ct = default)
        {
            rate.Id = Guid.NewGuid();
            Rates.Add(rate);
            return Task.FromResult(rate.Id);
        }
        public Task UpdateAsync(TaxRate rate, System.Threading.CancellationToken ct = default)
        {
            Updated = rate;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task LoadAsync_FillsCollection()
    {
        var service = new FakeService();
        service.Rates.Add(new TaxRate { Id = Guid.NewGuid(), Percentage = 27 });
        var vm = new TaxRateMasterViewModel(service);

        await vm.LoadAsync();

        Assert.Single(vm.TaxRates);
    }

    [Fact]
    public async Task DeleteSelectedCommand_ArchivesItem()
    {
        var service = new FakeService();
        var rate = new TaxRate { Id = Guid.NewGuid(), Percentage = 10 };
        service.Rates.Add(rate);
        var vm = new TaxRateMasterViewModel(service);
        await vm.LoadAsync();

        vm.SelectedItem = vm.TaxRates[0];
        vm.DeleteSelectedCommand.Execute(null);
        await Task.Delay(10);

        Assert.True(rate.IsArchived);
        Assert.Equal(rate, service.Updated);
    }
}
