using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.InlineCreators;

public partial class UnitCreatorView : UserControl
{
    public UnitCreatorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
