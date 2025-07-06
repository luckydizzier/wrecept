using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Tests.ViewModels;

public class PaymentMethodMasterViewModelTests
{
    private class FakePaymentMethodService : IPaymentMethodService
    {
        public List<PaymentMethod> Methods { get; } = new();
        public PaymentMethod? Updated;
        public Task<List<PaymentMethod>> GetAllAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Methods);
        public Task<List<PaymentMethod>> GetActiveAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Methods);
        public Task<Guid> AddAsync(PaymentMethod method, System.Threading.CancellationToken ct = default)
        {
            method.Id = Guid.NewGuid();
            Methods.Add(method);
            return Task.FromResult(method.Id);
        }
        public Task UpdateAsync(PaymentMethod method, System.Threading.CancellationToken ct = default)
        {
            Updated = method;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task LoadAsync_FillsCollection()
    {
        var service = new FakePaymentMethodService();
        service.Methods.Add(new PaymentMethod { Id = Guid.NewGuid(), Name = "Cash" });
        var vm = new PaymentMethodMasterViewModel(service);

        await vm.LoadAsync();

        Assert.Single(vm.PaymentMethods);
    }

    [Fact]
    public async Task DeleteSelectedCommand_ArchivesItem()
    {
        var service = new FakePaymentMethodService();
        var method = new PaymentMethod { Id = Guid.NewGuid(), Name = "X" };
        service.Methods.Add(method);
        var vm = new PaymentMethodMasterViewModel(service);
        await vm.LoadAsync();

        vm.SelectedItem = vm.PaymentMethods[0];
        vm.DeleteSelectedCommand.Execute(null);
        await Task.Delay(10);

        Assert.True(method.IsArchived);
        Assert.Equal(method, service.Updated);
    }
}
