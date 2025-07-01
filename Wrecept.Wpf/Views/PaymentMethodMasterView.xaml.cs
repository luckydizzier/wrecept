using Microsoft.Extensions.DependencyInjection;
using Wrecept.Wpf.ViewModels;
using Wrecept.Wpf.Views.Controls;

namespace Wrecept.Wpf.Views;

public partial class PaymentMethodMasterView : BaseMasterView<PaymentMethodMasterViewModel>
{
    public PaymentMethodMasterView() : this(App.Provider.GetRequiredService<PaymentMethodMasterViewModel>())
    {
    }

    public PaymentMethodMasterView(PaymentMethodMasterViewModel viewModel)
    {
        InitializeComponent();
        InitializeViewModel(viewModel);
    }
}
