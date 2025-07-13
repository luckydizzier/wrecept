using System;
using Xunit;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Tests.ViewModels;

public class EditableItemViewModelTests
{
    private static T CreateUninitialized<T>() => (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T));
    [Fact]
    public void ExistingItem_CopiesProperties()
    {
        var parent = CreateUninitialized<InvoiceEditorViewModel>();
        var source = new InvoiceItemRowViewModel(parent)
        {
            Product = "P",
            Quantity = 2,
            UnitPrice = 5,
            TaxRateId = Guid.NewGuid(),
            UnitId = Guid.NewGuid(),
            UnitName = "u",
            TaxRateName = "t",
            ProductGroup = "g",
            Description = "d"
        };
        var vm = new ExistingLineItemEditViewModel(parent, source);
        Assert.True(vm.IsEditingExisting);
        Assert.Equal(source.Product, vm.Product);
        Assert.Equal(source.Quantity, vm.Quantity);
        Assert.Equal(source.UnitPrice, vm.UnitPrice);
        Assert.Equal(source.TaxRateId, vm.TaxRateId);
        Assert.Equal(source.UnitId, vm.UnitId);
        Assert.Equal(source.UnitName, vm.UnitName);
        Assert.Equal(source.TaxRateName, vm.TaxRateName);
        Assert.Equal(source.ProductGroup, vm.ProductGroup);
        Assert.Equal(source.Description, vm.Description);
    }
}
