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
        if (e.Key == Key.Enter && Grid.SelectedIndex == 0)
        {
            vm.AddLineItemCommand.Execute(null);
            e.Handled = true;
        }
        else if (e.Key == Key.Enter && Grid.SelectedIndex > 0 && Grid.SelectedItem is InvoiceItemRowViewModel row)
        {
            vm.EditLineFromSelection(row);
            Grid.SelectedIndex = 0;
            e.Handled = true;
        }
        else if (e.Key == Key.Down && Grid.SelectedIndex == 0 && Grid.Items.Count > 1)
        {
            Grid.SelectedIndex = 1;
            e.Handled = true;
        }
    }
}
