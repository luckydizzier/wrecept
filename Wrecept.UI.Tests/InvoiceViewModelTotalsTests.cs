using System.Linq;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using Wrecept.UI.Services;
using Wrecept.UI.ViewModels;

namespace Wrecept.UI.Tests;

public class InvoiceViewModelTotalsTests
{
    private class StubInvoiceService : IInvoiceService
    {
        public Task AddInvoiceAsync(Invoice invoice) => Task.CompletedTask;
        public Task<IEnumerable<Invoice>> GetInvoicesAsync() => Task.FromResult<IEnumerable<Invoice>>(Array.Empty<Invoice>());
    }

    private class StubProductLookupService : IProductLookupService
    {
        public Task<IReadOnlyList<Product>> SearchAsync(string term) => Task.FromResult<IReadOnlyList<Product>>(Array.Empty<Product>());
    }

    private class StubTaxService : ITaxService
    {
        public Task<IReadOnlyList<decimal>> GetRatesAsync() => Task.FromResult<IReadOnlyList<decimal>>(new[] { 0.27m, 0.05m });
    }

    private class StubSettingsService : ISettingsService
    {
        public event EventHandler<ApplicationSettings>? SettingsChanged;
        public Task<ApplicationSettings> LoadAsync() => Task.FromResult(new ApplicationSettings());
        public Task SaveAsync(ApplicationSettings settings) => Task.CompletedTask;
        public Task UpdateThemeAsync(string theme) => Task.CompletedTask;
        public Task UpdateLanguageAsync(string language) => Task.CompletedTask;
    }

    private class StubMessageService : IMessageService
    {
        public void Show(string message, string? title = null) { }
        public bool Confirm(string message, string? title = null) => true;
    }

    private InvoiceViewModel CreateVm()
        => new(new StubInvoiceService(), new StubProductLookupService(), new StubTaxService(), new StubSettingsService(), new StubMessageService());

    [Fact]
    public void RecalculateTotals_ComputesValues()
    {
        var vm = CreateVm();
        vm.Items.Add(new InvoiceItemVM { Quantity = 2m, UnitPrice = 100m, TaxRate = 0.27m });
        vm.Items.Add(new InvoiceItemVM { Quantity = 1m, UnitPrice = 50m, TaxRate = 0.05m });

        vm.RecalculateTotals();

        Assert.Equal(250m, vm.TotalNet);
        Assert.Equal(56.5m, vm.TotalVat);
        Assert.Equal(306.5m, vm.TotalGross);
    }

    [Fact]
    public void VatTotals_GroupedByRate()
    {
        var vm = CreateVm();
        vm.Items.Add(new InvoiceItemVM { Quantity = 1m, UnitPrice = 100m, TaxRate = 0.27m });
        vm.Items.Add(new InvoiceItemVM { Quantity = 1m, UnitPrice = 50m, TaxRate = 0.27m });
        vm.Items.Add(new InvoiceItemVM { Quantity = 1m, UnitPrice = 20m, TaxRate = 0.05m });

        vm.RecalculateTotals();

        var rate27 = vm.VatTotals.First(v => v.Rate == 0.27m);
        var rate5 = vm.VatTotals.First(v => v.Rate == 0.05m);

        Assert.Equal(40.5m, rate27.Vat);
        Assert.Equal(1m, rate5.Vat);
    }
}

