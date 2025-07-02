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
        if (Grid.SelectedItem is InvoiceItemRowViewModel row)
        {
            if (e.Key == Key.Enter)
            {
                vm.EditLineFromSelection(row);
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                vm.RequestDeleteItem(row);
                e.Handled = true;
            }
            else if (e.Key is not (Key.Up or Key.Down))
            {
                NavigationHelper.Handle(e);
            }
        }
        else
        {
            if (e.Key is not (Key.Up or Key.Down))
                NavigationHelper.Handle(e);
        }
    }
}
