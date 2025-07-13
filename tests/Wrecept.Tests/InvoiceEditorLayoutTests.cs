using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Wpf.Services;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceEditorLayoutTests
{
    private static void EnsureApp()
    {
        if (Application.Current == null)
            new Application();
    }

    private class CountingService : IPaymentMethodService, ITaxRateService, ISupplierService, IProductService, IUnitService, IProductGroupService
    {
        public int Calls;
        public TaskCompletionSource<bool> Gate = new();

        private async Task<List<T>> Delay<T>()
        {
            await Gate.Task;
            return new();
        }

        public Task<List<PaymentMethod>> GetActiveAsync(System.Threading.CancellationToken ct = default) { Calls++; return Delay<PaymentMethod>(); }
        public Task<List<PaymentMethod>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<PaymentMethod>());
        public Task<Guid> AddAsync(PaymentMethod method, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(PaymentMethod method, System.Threading.CancellationToken ct = default) => Task.CompletedTask;

        public Task<List<TaxRate>> GetActiveAsync(DateTime asOf, System.Threading.CancellationToken ct = default) { Calls++; return Delay<TaxRate>(); }
        public Task<List<TaxRate>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<TaxRate>());
        public Task<Guid> AddAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(TaxRate taxRate, System.Threading.CancellationToken ct = default) => Task.CompletedTask;

        public Task<List<Supplier>> GetActiveAsync(System.Threading.CancellationToken ct = default) { Calls++; return Delay<Supplier>(); }
        public Task<List<Supplier>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Supplier>());
        public Task<int> AddAsync(Supplier supplier, System.Threading.CancellationToken ct = default) => Task.FromResult(1);
        public Task UpdateAsync(Supplier supplier, System.Threading.CancellationToken ct = default) => Task.CompletedTask;

        public Task<List<Product>> GetActiveAsync(System.Threading.CancellationToken ct = default) { Calls++; return Delay<Product>(); }
        public Task<List<Product>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Product>());
        public Task<int> AddAsync(Product product, System.Threading.CancellationToken ct = default) => Task.FromResult(1);
        public Task UpdateAsync(Product product, System.Threading.CancellationToken ct = default) => Task.CompletedTask;

        public Task<List<Unit>> GetActiveAsync(System.Threading.CancellationToken ct = default) { Calls++; return Delay<Unit>(); }
        public Task<List<Unit>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Unit>());
        public Task<Guid> AddAsync(Unit unit, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(Unit unit, System.Threading.CancellationToken ct = default) => Task.CompletedTask;

        public Task<List<ProductGroup>> GetActiveAsync(System.Threading.CancellationToken ct = default) { Calls++; return Delay<ProductGroup>(); }
        public Task<List<ProductGroup>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<Guid> AddAsync(ProductGroup group, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(ProductGroup group, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class DummyInvoiceService : IInvoiceService
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

    private class DummySessionService : ISessionService
    {
        public Task<int?> LoadLastInvoiceIdAsync(System.Threading.CancellationToken ct = default) => Task.FromResult<int?>(null);
        public Task SaveLastInvoiceIdAsync(int? invoiceId, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    [StaFact]
    public async Task Loaded_OpensProgressAndCallsLoadOnce()
    {
        EnsureApp();
        var svc = new CountingService();
        var invoice = new DummyInvoiceService();
        var numberSvc = new FakeNumberingService();
        var lookup = new InvoiceLookupViewModel(invoice, numberSvc);
        var vm = new InvoiceEditorViewModel(svc, svc, svc, svc, svc, svc, invoice, new DummyLogService(), new DummyNotificationService(), new DummySessionService(), new AppStateService(System.IO.Path.GetTempFileName()), lookup, numberSvc);
        var layout = new InvoiceEditorLayout(vm);

        layout.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        await Task.Yield();
        Assert.Single(Application.Current.Windows.OfType<StartupWindow>());
        svc.Gate.SetResult(true);
        await Dispatcher.Yield(DispatcherPriority.Background);
        Assert.Empty(Application.Current.Windows.OfType<StartupWindow>());
        Assert.Equal(6, svc.Calls);
    }
}
