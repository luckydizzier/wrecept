using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Collections.ObjectModel;

namespace Wrecept.Tests.ViewModels;

public class InvoiceEditorViewModelTests
{
    private class FakeInvoiceService : IInvoiceService
    {
        public readonly List<Invoice> Invoices = new();
        public readonly List<InvoiceItem> Items = new();
        private int _id = 1;
        public Task<bool> CreateAsync(Invoice invoice, System.Threading.CancellationToken ct = default)
        {
            invoice.Id = _id++;
            Invoices.Add(invoice);
            return Task.FromResult(true);
        }
        public Task<int> CreateHeaderAsync(Invoice invoice, System.Threading.CancellationToken ct = default)
        {
            invoice.Id = _id++;
            Invoices.Add(invoice);
            return Task.FromResult(invoice.Id);
        }
        public Task<int> AddItemAsync(InvoiceItem item, System.Threading.CancellationToken ct = default)
        {
            item.Id = _id++;
            Items.Add(item);
            return Task.FromResult(item.Id);
        }
        public Task UpdateInvoiceHeaderAsync(int id, DateOnly date, int supplierId, Guid paymentMethodId, bool isGross, System.Threading.CancellationToken ct = default)
        {
            UpdatedId = id;
            return Task.CompletedTask;
        }
        public Task ArchiveAsync(int id, System.Threading.CancellationToken ct = default)
        {
            Archived = true;
            return Task.CompletedTask;
        }
        public Task<Invoice?> GetAsync(int id, System.Threading.CancellationToken ct = default) => Task.FromResult<Invoice?>(null);
        public Task<List<Invoice>> GetRecentAsync(int count, System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Invoice>(Invoices));

        public InvoiceCalculationResult RecalculateTotals(Invoice invoice)
            => new InvoiceCalculator().Calculate(invoice);

        public int UpdatedId;
        public bool Archived;
    }

    private class FakeProductService : IProductService
    {
        public readonly List<Product> Products = new();
        public Task<List<Product>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Product>(Products));
        public Task<List<Product>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<Product>(Products));
        public Task<int> AddAsync(Product product, System.Threading.CancellationToken ct = default)
        {
            product.Id = Products.Count + 1;
            Products.Add(product);
            return Task.FromResult(product.Id);
        }
        public Task UpdateAsync(Product product, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class DummyService<T> : IPaymentMethodService, ITaxRateService, ISupplierService, IUnitService
        where T : class
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
    }

    private class DummyLogService : ILogService
    {
        public Exception? Logged;
        public Task LogError(string message, Exception ex)
        {
            Logged = ex;
            return Task.CompletedTask;
        }
    }

    private class DummyNotificationService : INotificationService
    {
        public string? LastError;
        public void ShowError(string message) => LastError = message;
        public void ShowInfo(string message) { }
        public bool Confirm(string message) => true;
    }

    [Fact]
    public async Task InlinePrompt_CreatesInvoice()
    {
        var invoiceSvc = new FakeInvoiceService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc);
        var prompt = new InvoiceCreatePromptViewModel(lookup, "INV1");

        await prompt.ConfirmAsyncCommand.ExecuteAsync(null);

        Assert.Single(invoiceSvc.Invoices);
        Assert.Null(lookup.InlinePrompt);
    }

    [Fact]
    public async Task AddLineItem_InvalidQuantity_Rejected()
    {
        var invoiceSvc = new FakeInvoiceService();
        var productSvc = new FakeProductService();
        productSvc.Products.Add(new Product { Id = 1, Name = "Test", TaxRateId = Guid.NewGuid() });
        var payment = new DummyService<object>();
        var tax = new DummyService<object>();
        var supplier = new DummyService<object>();
        var unit = new DummyService<object>();
        var log = new DummyLogService();
        var notify = new DummyNotificationService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc);
        var vm = new InvoiceEditorViewModel(payment, tax, supplier, productSvc, unit, invoiceSvc, log, notify, lookup);

        var row = vm.Items[0];
        row.Product = "Test";
        row.Quantity = 0;
        row.TaxRateId = productSvc.Products[0].TaxRateId;

        await vm.AddLineItemAsync();

        Assert.Equal(0, invoiceSvc.Items.Count);
        Assert.True(row.HasError);
    }

    [Fact]
    public async Task Commands_Ignored_When_ReadOnly()
    {
        var invoiceSvc = new FakeInvoiceService();
        var productSvc = new FakeProductService();
        productSvc.Products.Add(new Product { Id = 1, Name = "Test", TaxRateId = Guid.NewGuid() });
        var dummy = new DummyService<object>();
        var log = new DummyLogService();
        var notify = new DummyNotificationService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc);
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, invoiceSvc, log, notify, lookup)
        {
            IsArchived = true
        };

        var row = vm.Items[0];
        row.Product = "Test";
        row.Quantity = 1;
        row.TaxRateId = productSvc.Products[0].TaxRateId;

        await vm.AddLineItemAsync();
        Assert.Empty(invoiceSvc.Items);
        Assert.True(row.HasError);

        vm.EditLineFromSelection(new InvoiceItemRowViewModel(vm) { Product = "X" });
        Assert.Equal(string.Empty, vm.Items[0].Product);
    }

    [Fact]
    public async Task InlineProductCreation_AddsLineAutomatically()
    {
        var invoiceSvc = new FakeInvoiceService();
        var productSvc = new FakeProductService();
        var dummy = new DummyService<object>();
        var log = new DummyLogService();
        var notify = new DummyNotificationService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc);
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, invoiceSvc, log, notify, lookup);

        var row = vm.Items[0];
        var creator = new ProductCreatorViewModel(vm, row, productSvc)
        {
            Name = "NewProd",
            UnitId = Guid.NewGuid(),
            TaxRateId = Guid.NewGuid()
        };

        await creator.ConfirmAsync();

        Assert.Single(invoiceSvc.Items);
        Assert.Null(vm.InlineCreator);
    }

    [Fact]
    public async Task ArchiveCommand_DisablesEditing()
    {
        var invoiceSvc = new FakeInvoiceService();
        var productSvc = new FakeProductService();
        var dummy = new DummyService<object>();
        var log = new DummyLogService();
        var notify = new DummyNotificationService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc);
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, invoiceSvc, log, notify, lookup)
        {
            IsNew = false,
            InvoiceId = 1
        };

        await vm.ArchiveAsync();

        Assert.True(vm.IsArchived);
        Assert.False(vm.IsEditable);

        var row = vm.Items[0];
        row.Product = "Test";
        row.Quantity = 1;
        row.TaxRateId = Guid.NewGuid();

        await vm.AddLineItemAsync();

        Assert.Empty(invoiceSvc.Items);
    }

    [Fact]
    public async Task Totals_Recalculated_After_AddLine()
    {
        var invoiceSvc = new FakeInvoiceService();
        var productSvc = new FakeProductService();
        var taxId = Guid.NewGuid();
        productSvc.Products.Add(new Product { Id = 1, Name = "Test", TaxRateId = taxId });
        var dummy = new DummyService<object>();
        var log = new DummyLogService();
        var notify = new DummyNotificationService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc);
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, invoiceSvc, log, notify, lookup)
        {
            IsNew = false,
            InvoiceId = 1,
            IsGross = false
        };
        vm.TaxRates.Add(new TaxRate { Id = taxId, Percentage = 27 });

        var row = vm.Items[0];
        row.Product = "Test";
        row.Quantity = 2;
        row.UnitPrice = 100;
        row.TaxRateId = taxId;

        await vm.AddLineItemAsync();

        Assert.Equal(200, vm.NetTotal);
        Assert.Equal(54, vm.VatTotal);
        Assert.Equal(254, vm.GrossTotal);
    }

}
