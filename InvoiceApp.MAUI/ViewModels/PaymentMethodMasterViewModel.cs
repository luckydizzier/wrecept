using InvoiceApp.Core.Models;
using InvoiceApp.Core.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using InvoiceApp.MAUI.Services;

namespace InvoiceApp.MAUI.ViewModels;

public partial class PaymentMethodMasterViewModel : EditableMasterDataViewModel<PaymentMethod>
{
    public ObservableCollection<PaymentMethod> PaymentMethods => Items;
    private readonly IPaymentMethodService _service;

    public PaymentMethodMasterViewModel(IPaymentMethodService service, AppStateService state)
        : base(state)
    {
        _service = service;
    }

    protected override Task<List<PaymentMethod>> GetItemsAsync()
        => _service.GetActiveAsync();

    protected override async Task DeleteAsync()
    {
        if (SelectedItem != null)
        {
            SelectedItem.IsArchived = true;
            await _service.UpdateAsync(SelectedItem);
        }
    }
}
