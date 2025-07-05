using CommunityToolkit.Mvvm.ComponentModel;

namespace Wrecept.Wpf.ViewModels;

public partial class EditableItemViewModel : InvoiceItemRowViewModel
{
    public bool IsEditingExisting { get; internal set; }
    public InvoiceItemRowViewModel? TargetRow { get; internal set; }

    public EditableItemViewModel(InvoiceEditorViewModel parent) : base(parent)
    {
    }
}

public class NewLineItemViewModel : EditableItemViewModel
{
    public NewLineItemViewModel(InvoiceEditorViewModel parent) : base(parent)
    {
    }
}

public class ExistingLineItemEditViewModel : EditableItemViewModel
{
    public ExistingLineItemEditViewModel(InvoiceEditorViewModel parent, InvoiceItemRowViewModel target) : base(parent)
    {
        IsEditingExisting = true;
        TargetRow = target;
        Product = target.Product;
        Quantity = target.Quantity;
        UnitPrice = target.UnitPrice;
        TaxRateId = target.TaxRateId;
        UnitId = target.UnitId;
        UnitName = target.UnitName;
        TaxRateName = target.TaxRateName;
        ProductGroup = target.ProductGroup;
        Description = target.Description;
    }
}
