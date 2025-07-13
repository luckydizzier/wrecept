using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Tests.ViewModels;

public class PaymentMethodCreatorViewModelTests
{
    private static T CreateUninitialized<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));

    private class FakeService : IPaymentMethodService
    {
        public Task<List<PaymentMethod>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<List<PaymentMethod>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<Guid> AddAsync(PaymentMethod method, System.Threading.CancellationToken ct = default)
        {
            method.Id = Guid.NewGuid();
            return Task.FromResult(method.Id);
        }
        public Task UpdateAsync(PaymentMethod method, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    [Fact]
    public void Defaults_AreEmptyAndCommandsAvailable()
    {
        var parent = CreateUninitialized<InvoiceEditorViewModel>();
        var vm = new PaymentMethodCreatorViewModel(parent, new FakeService());

        Assert.Equal(string.Empty, vm.Name);
        Assert.Equal(0, vm.DueInDays);
        Assert.True(vm.ConfirmAsyncCommand.CanExecute(null));
        Assert.True(vm.CancelCommand.CanExecute(null));
    }
}
