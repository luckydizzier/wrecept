using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;

namespace Wrecept.Tests.ViewModels;

public class UnitCreatorViewModelTests
{
    private static T CreateUninitialized<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));

    private class FakeUnitService : IUnitService
    {
        public Task<List<Unit>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<List<Unit>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<Guid> AddAsync(Unit unit, System.Threading.CancellationToken ct = default)
        {
            unit.Id = Guid.NewGuid();
            return Task.FromResult(unit.Id);
        }
        public Task UpdateAsync(Unit unit, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    [Fact]
    public void Defaults_AreEmptyAndCommandsAvailable()
    {
        var parent = CreateUninitialized<InvoiceEditorViewModel>();
        var vm = new UnitCreatorViewModel(parent, new FakeUnitService());

        Assert.Equal(string.Empty, vm.Name);
        Assert.Equal(string.Empty, vm.Code);
        Assert.True(vm.ConfirmAsyncCommand.CanExecute(null));
        Assert.True(vm.CancelCommand.CanExecute(null));
    }
}
