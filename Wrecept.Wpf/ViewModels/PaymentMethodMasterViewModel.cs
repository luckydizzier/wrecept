using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wrecept.Wpf.ViewModels;

public partial class PaymentMethodMasterViewModel : MasterDataBaseViewModel<PaymentMethod>
{
    public ObservableCollection<PaymentMethod> PaymentMethods => Items;
    private readonly IPaymentMethodService _service;

    public PaymentMethodMasterViewModel(IPaymentMethodService service)
    {
        _service = service;
    }

    protected override Task<List<PaymentMethod>> GetItemsAsync()
        => _service.GetActiveAsync();
}
