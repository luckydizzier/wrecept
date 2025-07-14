using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;

namespace InvoiceApp.Tests.ViewModels;

public class UnitMasterViewModelTests
{
    private class FakeService : IUnitService
    {
        public List<Unit> Units { get; } = new();
        public Unit? Updated;
        public Task<List<Unit>> GetAllAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Units);
        public Task<List<Unit>> GetActiveAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Units);
        public Task<Guid> AddAsync(Unit unit, System.Threading.CancellationToken ct = default)
        {
            unit.Id = Guid.NewGuid();
            Units.Add(unit);
            return Task.FromResult(unit.Id);
        }
        public Task UpdateAsync(Unit unit, System.Threading.CancellationToken ct = default)
        {
            Updated = unit;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task LoadAsync_FillsCollection()
    {
        var service = new FakeService();
        service.Units.Add(new Unit { Id = Guid.NewGuid(), Name = "pc" });
        var vm = new UnitMasterViewModel(service);

        await vm.LoadAsync();

        Assert.Single(vm.Units);
    }

    [Fact]
    public async Task DeleteSelectedCommand_ArchivesItem()
    {
        var service = new FakeService();
        var unit = new Unit { Id = Guid.NewGuid(), Name = "kg" };
        service.Units.Add(unit);
        var vm = new UnitMasterViewModel(service);
        await vm.LoadAsync();

        vm.SelectedItem = vm.Units[0];
        vm.DeleteSelectedCommand.Execute(null);
        await service.WaitForUpdateAsync();

        Assert.True(unit.IsArchived);
        Assert.Equal(unit, service.Updated);
    }
}
