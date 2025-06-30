using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wrecept.Core.Models;
using Wrecept.Core.Services;
using System.Threading.Tasks;

namespace Wrecept.Wpf.ViewModels;

public partial class PaymentMethodMasterViewModel : ObservableObject
{
    public ObservableCollection<PaymentMethod> PaymentMethods { get; } = new();

    private readonly IPaymentMethodService _service;

    public PaymentMethodMasterViewModel(IPaymentMethodService service)
    {
        _service = service;
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();
        PaymentMethods.Clear();
        foreach (var item in items)
            PaymentMethods.Add(item);
    }
}
