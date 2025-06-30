using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.InlineCreators;

public partial class SupplierCreatorView : UserControl
{
    public SupplierCreatorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
