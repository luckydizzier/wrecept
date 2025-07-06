using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Services;
using Wrecept.Core.Services;
using Wrecept.Core.Models;
using Wrecept.Core.Entities;
using Wrecept.Core.Enums;

namespace Wrecept.Tests.ViewModels;

public class StageViewModelTests
{
    private static T CreateUninitialized<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));

    private class FakeDbHealth : IDbHealthService
    {
        public int Calls;
        public Task<bool> CheckAsync(System.Threading.CancellationToken ct = default)
        {
            Calls++;
            return Task.FromResult(true);
        }
    }

    private class FakeSession : ISessionService
    {
        public int? Last;
        public Task<int?> LoadLastInvoiceIdAsync(System.Threading.CancellationToken ct = default)
            => Task.FromResult(Last);
        public Task SaveLastInvoiceIdAsync(int? invoiceId, System.Threading.CancellationToken ct = default)
        {
            Last = invoiceId;
            return Task.CompletedTask;
        }
    }

    private class FakeUserInfoService : IUserInfoService
    {
        public Task<UserInfo> LoadAsync() => Task.FromResult(new UserInfo());
        public Task SaveAsync(UserInfo info) => Task.CompletedTask;
    }

    private StageViewModel Create(StageMenuAction last)
    {
        var state = new AppStateService(Path.GetTempFileName())
        {
            LastView = last,
            CurrentInvoiceId = null
        };
        var invoice = new InvoiceEditorViewModel(); // Replace with proper instantiation logic.
        var product = new ProductMasterViewModel(new FakeProductService(), new FakeTaxRateService());
        var group = new ProductGroupMasterViewModel(new FakeProductGroupService());
        var supplier = new SupplierMasterViewModel(new FakeSupplierService());
        var tax = new TaxRateMasterViewModel(new FakeTaxRateService());
        var payment = new PaymentMethodMasterViewModel(new FakePaymentMethodService());
        var unit = new UnitMasterViewModel(new FakeUnitService());
        var user = new UserInfoViewModel(new FakeUserInfoService());
        var about = new AboutViewModel(new FakeUserInfoService());
        var placeholder = new PlaceholderViewModel();
        var status = new StatusBarViewModel();
        var db = new FakeDbHealth();
        var session = new FakeSession();

        return new StageViewModel(invoice, product, group, supplier, tax, payment, unit, user, about, placeholder, status, db, session, state);
    }

    private class FakeProductService : IProductService
    {
        public List<Product> Products { get; } = new();
        public Task<List<Product>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Products);
        public Task<List<Product>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Products);
        public Task<int> AddAsync(Product product, System.Threading.CancellationToken ct = default) => Task.FromResult(0);
        public Task UpdateAsync(Product product, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class FakeProductGroupService : IProductGroupService
    {
        public List<ProductGroup> Groups { get; } = new();
        public Task<List<ProductGroup>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Groups);
        public Task<List<ProductGroup>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Groups);
        public Task<Guid> AddAsync(ProductGroup group, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(ProductGroup group, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class FakeSupplierService : ISupplierService
    {
        public List<Supplier> Suppliers { get; } = new();
        public Task<List<Supplier>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Suppliers);
        public Task<List<Supplier>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Suppliers);
        public Task<int> AddAsync(Supplier supplier, System.Threading.CancellationToken ct = default) => Task.FromResult(0);
        public Task UpdateAsync(Supplier supplier, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class FakeTaxRateService : ITaxRateService
    {
        public List<TaxRate> Rates { get; } = new();
        public Task<List<TaxRate>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Rates);
        public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, System.Threading.CancellationToken ct = default) => Task.FromResult(Rates);
        public Task<Guid> AddAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class FakePaymentMethodService : IPaymentMethodService
    {
        public List<PaymentMethod> Methods { get; } = new();
        public Task<List<PaymentMethod>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Methods);
        public Task<List<PaymentMethod>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Methods);
        public Task<Guid> AddAsync(PaymentMethod method, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(PaymentMethod method, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class FakeUnitService : IUnitService
    {
        public List<Unit> Units { get; } = new();
        public Task<List<Unit>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Units);
        public Task<List<Unit>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(Units);
        public Task<Guid> AddAsync(Unit unit, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(Unit unit, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    [StaFact]
    public void Constructor_RespectsSavedView()
    {
        var vm = Create(StageMenuAction.EditSuppliers);
        Assert.IsType<SupplierMasterViewModel>(vm.CurrentViewModel);
    }

    [StaFact]
    public async Task HandleMenuCommand_SwitchesViewAndState()
    {
        var state = new AppStateService(Path.GetTempFileName());
        var invoice = new InvoiceEditorViewModel();
        var vm = new StageViewModel(
            invoice,
            new ProductMasterViewModel(new FakeProductService(), new FakeTaxRateService(), state),
            new ProductGroupMasterViewModel(new FakeProductGroupService(), state),
            new SupplierMasterViewModel(new FakeSupplierService(), state),
            new TaxRateMasterViewModel(new FakeTaxRateService(), state),
            new PaymentMethodMasterViewModel(new FakePaymentMethodService(), state),
            new UnitMasterViewModel(new FakeUnitService(), state),
            new UserInfoViewModel(new FakeUserInfoService()),
            new AboutViewModel(new FakeUserInfoService()),
            new PlaceholderViewModel(),
            new StatusBarViewModel(),
            new FakeDbHealth(),
            new FakeSession(),
            state);

        await vm.HandleMenuCommand.ExecuteAsync(StageMenuAction.EditUnits);
        Assert.IsType<UnitMasterViewModel>(vm.CurrentViewModel);
        Assert.Equal(AppInteractionState.EditingMasterData, state.InteractionState);
    }

    [StaFact]
    public async Task HandleMenu_UserInfo_TemporarilyOpensDialog()
    {
        var state = new AppStateService(Path.GetTempFileName());
        var invoice = new InvoiceEditorViewModel();
        var changes = new List<AppInteractionState>();
        state.InteractionStateChanged += s => changes.Add(s);
        var vm = new StageViewModel(
            invoice,
            new ProductMasterViewModel(new FakeProductService(), new FakeTaxRateService(), state),
            new ProductGroupMasterViewModel(new FakeProductGroupService(), state),
            new SupplierMasterViewModel(new FakeSupplierService(), state),
            new TaxRateMasterViewModel(new FakeTaxRateService(), state),
            new PaymentMethodMasterViewModel(new FakePaymentMethodService(), state),
            new UnitMasterViewModel(new FakeUnitService(), state),
            new UserInfoViewModel(new FakeUserInfoService()),
            new AboutViewModel(new FakeUserInfoService()),
            new PlaceholderViewModel(),
            new StatusBarViewModel(),
            new FakeDbHealth(),
            new FakeSession(),
            state);

        await vm.HandleMenuCommand.ExecuteAsync(StageMenuAction.UserInfo);

        Assert.Contains(AppInteractionState.DialogOpen, changes);
        Assert.Equal(AppInteractionState.MainMenu, state.InteractionState);
    }
}
