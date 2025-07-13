using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using InvoiceApp.MAUI.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using InvoiceApp.MAUI.Services;

namespace Wrecept.Tests.ViewModels;

public class DeleteItemPromptViewModelTests
{
    private class FakeInvoiceService : IInvoiceService
    {
        public Task<bool> CreateAsync(Invoice invoice, System.Threading.CancellationToken ct = default) => Task.FromResult(true);
        public Task<int> CreateHeaderAsync(Invoice invoice, System.Threading.CancellationToken ct = default) => Task.FromResult(1);
        public Task<int> AddItemAsync(InvoiceItem item, System.Threading.CancellationToken ct = default) => Task.FromResult(1);
        public Task UpdateInvoiceHeaderAsync(int id, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
        public Task ArchiveAsync(int id, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
        public Task<Invoice?> GetAsync(int id, System.Threading.CancellationToken ct = default) => Task.FromResult<Invoice?>(null);
        public Task<List<Invoice>> GetRecentAsync(int count, System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Invoice>());
        public InvoiceCalculationResult RecalculateTotals(Invoice invoice) => new();
    }

    private class DummyService<T> : IPaymentMethodService, ITaxRateService, ISupplierService, IUnitService, IProductGroupService where T : class
    {
        public Task<List<PaymentMethod>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<List<PaymentMethod>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<Guid> AddAsync(PaymentMethod method, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(PaymentMethod method, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
        public Task<List<TaxRate>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, System.Threading.CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<Guid> AddAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
        public Task<List<Supplier>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<List<Supplier>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<int> AddAsync(Supplier supplier, System.Threading.CancellationToken ct = default) => Task.FromResult(1);
        public Task UpdateAsync(Supplier supplier, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
        public Task<List<Unit>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<List<Unit>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<Guid> AddAsync(Unit unit, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(Unit unit, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
        public Task<List<ProductGroup>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<List<ProductGroup>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<Guid> AddAsync(ProductGroup group, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(ProductGroup group, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class DummyLogService : ILogService
    {
        public Task LogError(string message, Exception ex) => Task.CompletedTask;
    }

    private class DummyNotificationService : INotificationService
    {
        public void ShowError(string message) { }
        public void ShowInfo(string message) { }
        public bool Confirm(string message) => true;
    }

    private static InvoiceEditorViewModel CreateEditor()
    {
        var invoice = new FakeInvoiceService();
        var numberSvc = new FakeNumberingService();
        var lookup = new InvoiceLookupViewModel(invoice, numberSvc);
        var state = new AppStateService(Path.GetTempFileName());
        return new InvoiceEditorViewModel(new DummyService<object>(), new DummyService<object>(), new DummyService<object>(), new FakeProductService(), new DummyService<object>(), new DummyService<object>(), invoice, new DummyLogService(), new DummyNotificationService(), state, lookup, numberSvc);
    }

    [Fact]
    public void Confirm_RemovesRowAndCloses()
    {
        var vm = CreateEditor();
        vm.Items.Insert(1, new InvoiceItemRowViewModel(vm) { Product = "X" });
        var row = vm.Items[1];
        var prompt = new DeleteItemPromptViewModel(vm, row);
        vm.DeletePrompt = prompt;

        prompt.ConfirmCommand.Execute(null);

        Assert.Equal(1, vm.Items.Count);
        Assert.Null(vm.DeletePrompt);
    }

    [Fact]
    public void Cancel_ClosesPrompt()
    {
        var vm = CreateEditor();
        var row = new InvoiceItemRowViewModel(vm);
        var prompt = new DeleteItemPromptViewModel(vm, row);
        vm.DeletePrompt = prompt;

        prompt.CancelCommand.Execute(null);

        Assert.Null(vm.DeletePrompt);
    }
}
