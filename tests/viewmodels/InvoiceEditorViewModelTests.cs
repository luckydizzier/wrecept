using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Xunit;
using Wrecept.Wpf.ViewModels;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.Wpf.Services;
using System.Collections.ObjectModel;
using System.Linq;
using Wrecept.Core.Enums;

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
        public Task UpdateInvoiceHeaderAsync(int id, string number, DateOnly date, DateOnly dueDate, int supplierId, Guid paymentMethodId, bool isGross, System.Threading.CancellationToken ct = default)
        {
            UpdatedId = id;
            var inv = Invoices.FirstOrDefault(i => i.Id == id);
            if (inv != null)
            {
                inv.Number = number;
                inv.Date = date;
                inv.DueDate = dueDate;
                inv.SupplierId = supplierId;
                inv.PaymentMethodId = paymentMethodId;
                inv.IsGross = isGross;
            }
            return Task.CompletedTask;
        }
        public Task ArchiveAsync(int id, System.Threading.CancellationToken ct = default)
        {
            Archived = true;
            return Task.CompletedTask;
        }
        public Task<Invoice?> GetAsync(int id, System.Threading.CancellationToken ct = default)
            => Task.FromResult(Invoices.FirstOrDefault(i => i.Id == id));
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

    private class DummyService<T> : IPaymentMethodService, ITaxRateService, ISupplierService, IUnitService, IProductGroupService
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

        public Task<List<ProductGroup>> GetAllAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<List<ProductGroup>> GetActiveAsync(System.Threading.CancellationToken ct = default) => Task.FromResult(new List<ProductGroup>());
        public Task<Guid> AddAsync(ProductGroup group, System.Threading.CancellationToken ct = default) => Task.FromResult(Guid.NewGuid());
        public Task UpdateAsync(ProductGroup group, System.Threading.CancellationToken ct = default) => Task.CompletedTask;
    }

    private class FakeNumberingService : INumberingService
    {
        private int _counter;
        public Task<string> GetNextInvoiceNumberAsync(System.Threading.CancellationToken ct = default)
        {
            _counter++;
            return Task.FromResult($"INV{_counter}");
        }
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
    public async Task AddLineItem_InvalidQuantity_Rejected()
    {
        var invoiceSvc = new FakeInvoiceService();
        var productSvc = new FakeProductService();
        productSvc.Products.Add(new Product { Id = 1, Name = "Test", TaxRateId = Guid.NewGuid() });
        var payment = new DummyService<object>();
        var tax = new DummyService<object>();
        var supplier = new DummyService<object>();
        var unit = new DummyService<object>();
        var groups = new DummyService<object>();
        var groups = new DummyService<object>();
        var log = new DummyLogService();
        var notify = new DummyNotificationService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(payment, tax, supplier, productSvc, unit, groups, invoiceSvc, log, notify, state, lookup);

        var row = vm.Items[0];
        row.Product = "Test";
        row.Quantity = 0;
        row.TaxRateId = productSvc.Products[0].TaxRateId;

        await vm.ItemsEditor.AddLineItemAsync();

        Assert.Equal(0, invoiceSvc.Items.Count);
        Assert.True(row.HasError);
    }

    [Fact]
    public async Task AddLineItem_NegativeQuantity_Accepted()
    {
        var invoiceSvc = new FakeInvoiceService();
        var productSvc = new FakeProductService();
        var taxId = Guid.NewGuid();
        productSvc.Products.Add(new Product { Id = 1, Name = "Test", TaxRateId = taxId });
        var payment = new DummyService<object>();
        var tax = new DummyService<object>();
        var supplier = new DummyService<object>();
        var unit = new DummyService<object>();
        var log = new DummyLogService();
        var notify = new DummyNotificationService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(payment, tax, supplier, productSvc, unit, groups, invoiceSvc, log, notify, state, lookup);

        var row = vm.Items[0];
        row.Product = "Test";
        row.Quantity = -2;
        row.UnitPrice = 100;
        row.TaxRateId = taxId;

        await vm.ItemsEditor.AddLineItemAsync();

        Assert.Equal(0, invoiceSvc.Items.Count);
        Assert.False(row.HasError);
        Assert.Equal(2, vm.Items.Count);
        Assert.Equal(-2, vm.Items[1].Quantity);
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
        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, dummy, invoiceSvc, log, notify, state, lookup)
        {
            IsArchived = true
        };

        var row = vm.Items[0];
        row.Product = "Test";
        row.Quantity = 1;
        row.TaxRateId = productSvc.Products[0].TaxRateId;

        await vm.ItemsEditor.AddLineItemAsync();
        Assert.Empty(invoiceSvc.Items);
        Assert.True(row.HasError);

        vm.ItemsEditor.EditLineFromSelection(new InvoiceItemRowViewModel(vm) { Product = "X" });
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
        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, dummy, invoiceSvc, log, notify, state, lookup);

        var row = vm.Items[0];
        var creator = new ProductCreatorViewModel(vm, row, productSvc)
        {
            Name = "NewProd",
            UnitId = Guid.NewGuid(),
            TaxRateId = Guid.NewGuid()
        };

        await creator.ConfirmAsync();

        Assert.Empty(invoiceSvc.Items);
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
        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, dummy, invoiceSvc, log, notify, state, lookup)
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

        await vm.ItemsEditor.AddLineItemAsync();

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
        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, dummy, invoiceSvc, log, notify, state, lookup)
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

        await vm.ItemsEditor.AddLineItemAsync();

        Assert.Equal(200, vm.Totals.NetTotal);
        Assert.Equal(54, vm.Totals.VatTotal);
        Assert.Equal(254, vm.Totals.GrossTotal);
    }

    [Fact]
    public void Selecting_Invoice_Loads_Header()
    {
        var invoiceSvc = new FakeInvoiceService();
        invoiceSvc.Invoices.Add(new Invoice { Id = 1, Number = "A1", Date = DateOnly.FromDateTime(DateTime.Today) });
        var dummy = new DummyService<object>();
        var productSvc = new FakeProductService();
        var log = new DummyLogService();
        var notify = new DummyNotificationService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(dummy, dummy, dummy, productSvc, dummy, dummy, invoiceSvc, log, notify, state, lookup);

        lookup.Invoices.Add(new InvoiceLookupItem { Id = 1, Number = "A1", Date = DateOnly.FromDateTime(DateTime.Today) });
        lookup.SelectedInvoice = lookup.Invoices[0];

        Assert.Equal(1, vm.InvoiceId);
        Assert.False(vm.IsNew);
    }

    [Fact]
    public void InlineCreator_SetsInteractionState()
    {
        var invoiceSvc = new FakeInvoiceService();
        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(new DummyService<object>(), new DummyService<object>(), new DummyService<object>(), new FakeProductService(), new DummyService<object>(), new DummyService<object>(), invoiceSvc, new DummyLogService(), new DummyNotificationService(), state, lookup);

        vm.InlineCreator = new ProductCreatorViewModel(vm, vm.Items[0], new FakeProductService());
        Assert.Equal(AppInteractionState.InlineCreatorActive, state.InteractionState);
        vm.InlineCreator = null;
        Assert.Equal(AppInteractionState.EditingInvoice, state.InteractionState);
    }

    [Fact]
    public async Task WithDialogOpen_RestoresState()
    {
        var state = new AppStateService(Path.GetTempFileName())
        {
            InteractionState = AppInteractionState.EditingInvoice
        };

        var during = AppInteractionState.None;

        await state.WithDialogOpen(async () =>
        {
            during = state.InteractionState;
            await Task.CompletedTask;
        });

        Assert.Equal(AppInteractionState.DialogOpen, during);
        Assert.Equal(AppInteractionState.EditingInvoice, state.InteractionState);
    }

    [Fact]
    public async Task SaveCommand_RefreshesLookupAndReselectsEditedInvoice()
    {
        var invoiceSvc = new FakeInvoiceService();
        var paymentId = Guid.NewGuid();
        var invoice = new Invoice
        {
            Id = 1,
            Number = "INV1",
            Date = DateOnly.FromDateTime(DateTime.Today),
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
            SupplierId = 1,
            PaymentMethodId = paymentId,
            IsGross = false
        };
        invoiceSvc.Invoices.Add(invoice);

        var lookup = new InvoiceLookupViewModel(invoiceSvc, new FakeNumberingService());
        await lookup.LoadAsync();
        var state = new AppStateService(Path.GetTempFileName());
        var vm = new InvoiceEditorViewModel(new DummyService<object>(), new DummyService<object>(), new DummyService<object>(), new FakeProductService(), new DummyService<object>(), new DummyService<object>(), invoiceSvc, new DummyLogService(), new DummyNotificationService(), state, lookup);

        lookup.SelectedInvoice = lookup.Invoices[0];

        var newPaymentId = Guid.NewGuid();
        vm.PaymentMethods.Add(new PaymentMethod { Id = newPaymentId, DueInDays = 0 });
        vm.Number = "NEW1";
        vm.SupplierId = 2;
        vm.PaymentMethodId = newPaymentId;
        vm.InvoiceDate = DateTime.Today;

        await vm.SaveCommand.ExecuteAsync(null);

        Assert.Equal("NEW1", lookup.SelectedInvoice?.Number);
        Assert.Equal(1, lookup.SelectedInvoice?.Id);
        var stored = invoiceSvc.Invoices.First(i => i.Id == 1);
        Assert.Equal(2, stored.SupplierId);
        Assert.Equal("NEW1", stored.Number);
    }

}
