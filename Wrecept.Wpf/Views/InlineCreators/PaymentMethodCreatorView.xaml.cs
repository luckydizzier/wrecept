using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Wrecept.Wpf.Views.InlineCreators;

public partial class PaymentMethodCreatorView : UserControl
{
    private readonly KeyboardManager? _keyboard;

    public PaymentMethodCreatorView()
    {
        if (App.Provider is not null)
            _keyboard = App.Provider.GetRequiredService<KeyboardManager>();
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => _keyboard?.Handle(e);
}
