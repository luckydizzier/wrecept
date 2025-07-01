using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Wpf.ViewModels;

namespace Wrecept.Wpf.Views;

public partial class InvoiceItemsGrid : UserControl
{
    public InvoiceItemsGrid()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not InvoiceEditorViewModel vm)
            return;
        if (!vm.IsEditable)
            return;
        if (e.Key == Key.Enter && Grid.SelectedItem is InvoiceItemRowViewModel row)
        {
            vm.EditLineFromSelection(row);
            e.Handled = true;
        }
        else
        {
            NavigationHelper.Handle(e);
        }
    }
}
