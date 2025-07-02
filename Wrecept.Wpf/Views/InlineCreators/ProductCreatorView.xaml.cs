using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Wpf.Views.InlineCreators;

public partial class ProductCreatorView : UserControl
{
    public ProductCreatorView()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is ViewModels.ProductCreatorViewModel vm && e.Key == Key.Escape)
        {
            vm.CloseEditorCommand.Execute(null);
            e.Handled = true;
            return;
        }
        NavigationHelper.Handle(e);
    }
}
