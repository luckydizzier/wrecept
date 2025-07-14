using System;
using System.Collections.ObjectModel;
using InvoiceApp.Core.Models;
using InvoiceApp.MAUI.ViewModels;
using Xunit;

namespace InvoiceApp.Tests.ViewModels;

public class TotalsViewModelTests
{
    [Fact]
    public void Recalculate_ComputesTotals()
    {
        var vm = new TotalsViewModel();
        var rows = new ObservableCollection<InvoiceItemRowViewModel>
        {
            new(new InvoiceEditorViewModel(null!, null!, null!, null!, null!, null!, null!, null!, null!, null!, null!, null!, null!))
            {
                Quantity = 2,
                UnitPrice = 100,
                TaxRateId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            }
        };
        var rates = new[] { new TaxRate { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Percentage = 27 } };

        vm.Recalculate(rows, rates, false);

        Assert.Equal(200, vm.NetTotal);
        Assert.Equal(54, vm.VatTotal);
        Assert.Equal(254, vm.GrossTotal);
    }
}
