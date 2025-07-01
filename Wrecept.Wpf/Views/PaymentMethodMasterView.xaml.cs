using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class PaymentMethodMasterView : BaseMasterView<PaymentMethodMasterViewModel>
{
    public PaymentMethodMasterView()
    {
    }

    public PaymentMethodMasterView(PaymentMethodMasterViewModel viewModel) : base(viewModel)
    {
    }
}
