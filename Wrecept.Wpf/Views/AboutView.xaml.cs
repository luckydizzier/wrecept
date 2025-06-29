using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views;

public partial class AboutView : UserControl
{
    public AboutView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
        => NavigationHelper.Handle(e);
}
