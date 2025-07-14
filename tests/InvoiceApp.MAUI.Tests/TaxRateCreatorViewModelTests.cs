using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Tests.ViewModels;

public class TaxRateCreatorViewModelTests
{
    private static T CreateUninitialized<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));

    private class FakeService : ITaxRateService
    {
        public Task<List<TaxRate>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, System.Threading.CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<Guid> AddAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default)
        {
            taxRate.Id = Guid.NewGuid();
            return Task.FromResult(taxRate.Id);
        }
        public Task UpdateAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    [Fact]
    public void Defaults_AreEmptyAndCommandsAvailable()
    {
        var parent = CreateUninitialized<InvoiceEditorViewModel>();
        var vm = new TaxRateCreatorViewModel(parent, new FakeService());

        Assert.Equal(string.Empty, vm.Name);
        Assert.Equal(string.Empty, vm.Code);
        Assert.Equal(0, vm.Percentage);
        Assert.True(vm.ConfirmAsyncCommand.CanExecute(null));
        Assert.True(vm.CancelCommand.CanExecute(null));
    }
}
